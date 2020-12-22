using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;

namespace WoodProject
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class WoodProjectApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {
            System.Console.WriteLine("OnStartup...");
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnShutdown(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        public void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            // Run the application logic.
            Process(e.DesignAutomationData);

            e.Succeeded = true;
        }

        private static void SetUnits(Document doc)
        {
            Units currentUnits = doc.GetUnits();
            FormatOptions nFt = new FormatOptions();
            nFt.UseDefault = false;
            nFt.DisplayUnits = DisplayUnitType.DUT_CENTIMETERS;
            currentUnits.SetFormatOptions(UnitType.UT_Length, nFt);
        }

        private static void Process(DesignAutomationData data)
        {
            if (data == null)
                throw new InvalidDataException(nameof(data));

            Application rvtApp = data.RevitApp;
            if (rvtApp == null)
                throw new InvalidDataException(nameof(rvtApp));

            System.Console.WriteLine("Start application...");

            System.Console.WriteLine("File Path" + data.FilePath);


            string modelPath = data.FilePath;

            if (String.IsNullOrWhiteSpace(modelPath))
            {
                System.Console.WriteLine("No File Path");
            }

            if (String.IsNullOrWhiteSpace(modelPath)) throw new InvalidDataException(nameof(modelPath));

            Document doc = data.RevitDoc;
            if (doc == null) throw new InvalidOperationException("Could not open document.");

            // SetUnits(doc);

            string filepathJson = "WoodProjectInput.json";
            WoodProjectItem jsonDeserialized = WoodProjectParams.Parse(filepathJson);

            CreateWalls(jsonDeserialized, doc);

            var saveAsOptions = new SaveAsOptions();
            saveAsOptions.OverwriteExistingFile = true;

            ModelPath path = ModelPathUtils.ConvertUserVisiblePathToModelPath("woodproject_result.rvt");

            doc.SaveAs(path, saveAsOptions);
        }



        private static void CreateWalls(WoodProjectItem jsonDeserialized, Document newDoc)
        {
            FilteredElementCollector levelCollector = new FilteredElementCollector(newDoc);
            levelCollector.OfClass(typeof(Level));
            var levelElements = levelCollector.ToElements();
            FilteredElementCollector levelCollector1 = new FilteredElementCollector(newDoc);
            levelCollector1.OfClass(typeof(Wall));
            var walls = levelCollector1.ToElements();
            if (levelElements == null || !levelElements.Any()) throw new InvalidDataException("ElementID is invalid.");
            var wallTypeId = newDoc.GetDefaultElementTypeId(ElementTypeGroup.WallType);
            if (wallTypeId == null || wallTypeId.IntegerValue < 0) throw new InvalidDataException("ElementID is invalid.");

            UnitConversionFactors unitFactor = new UnitConversionFactors("cm", "N");

            List<LevelInfo> levelInfos = new List<LevelInfo>();
            double currentElevation = 0;
            foreach (var floorItems in jsonDeserialized.Solutions
                .GroupBy(x => $"Level {x.Floor}")
                .OrderBy(x => x.Key))
            {
                if (!floorItems.Any())
                {
                    continue;
                }
                var firstFloorItem = floorItems.FirstOrDefault();
                if (firstFloorItem == null)
                {
                    continue;
                }

                var wallHeight = !firstFloorItem.Height.HasValue ||
                                 firstFloorItem.Height < 230 ? 230 :
                    firstFloorItem.Height > 244 ? 244 :
                    firstFloorItem.Height.Value;
                wallHeight = wallHeight / unitFactor.LengthRatio;
                var level = new LevelInfo
                {
                    Id = null,
                    Name = floorItems.Key,
                    Elevator = currentElevation,
                    Height = wallHeight,
                    Curves = new List<Curve>(),
                };

                if (levelElements.Select(x => x.Name).Contains(floorItems.Key))
                {
                    level.Id = levelElements.First(x => x.Name == floorItems.Key).Id;
                }

                level.Curves.AddRange(floorItems.Select(item =>
                        Line.CreateBound(
                            new XYZ(item.Sx / unitFactor.LengthRatio, item.Sy / unitFactor.LengthRatio, 0),
                            new XYZ(item.Ex / unitFactor.LengthRatio, item.Ey / unitFactor.LengthRatio, 0))
                    )
                );
                levelInfos.Add(level);
                currentElevation += wallHeight;
            }

            using (Transaction constructTrans = new Transaction(newDoc, "Create construct"))
            {
                constructTrans.Start();
                foreach (var levelInfo in levelInfos)
                {
                    var level = CreateLevel(newDoc, levelElements, levelInfo.Elevator, levelInfo);
                    if (level != null)
                    {
                        CreateFloor(newDoc, level, jsonDeserialized.Areas[0], unitFactor);
                        foreach (var curve in levelInfo.Curves)
                        {
                            Wall.Create(newDoc, curve, wallTypeId, level.Id, levelInfo.Height, 0, false, false);
                        }
                    }
                }

                constructTrans.Commit();
            }
        }
        private static Level CreateLevel(Document document, IList<Element> levelElements, double elevation,
            LevelInfo levelInfo)
        {
            if (levelInfo.Id != null &&
                levelElements.FirstOrDefault(x => x.Id == levelInfo.Id) is Level levelElement) // existed level
            {
                levelElement.Elevation = elevation;
                return levelElement;
            }
            
            // Begin to create a level
            Level level = Level.Create(document, elevation);
            
            if (null == level)
            {
                throw new Exception("Create a new level failed.");
            }

            return level;
        }

        private static Floor CreateFloor(Document document, Level level, Area area, UnitConversionFactors unitFactor)
        {
            // Get a floor type for floor creation
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfClass(typeof(FloorType));
            FloorType floorType = collector.FirstElement() as FloorType;

            CurveArray profile = new CurveArray();
            profile.Append(Line.CreateBound(
                new XYZ(area.Xmax / unitFactor.LengthRatio, area.Ymax / unitFactor.LengthRatio, 0),
                new XYZ(area.Xmax / unitFactor.LengthRatio, area.Ymin / unitFactor.LengthRatio, 0)));
            profile.Append(Line.CreateBound(
                new XYZ(area.Xmax / unitFactor.LengthRatio, area.Ymin / unitFactor.LengthRatio, 0),
                new XYZ(area.Xmin / unitFactor.LengthRatio, area.Ymin / unitFactor.LengthRatio, 0)));
            profile.Append(Line.CreateBound(
                new XYZ(area.Xmin / unitFactor.LengthRatio, area.Ymin / unitFactor.LengthRatio, 0),
                new XYZ(area.Xmin / unitFactor.LengthRatio, area.Ymax / unitFactor.LengthRatio, 0)));
            profile.Append(Line.CreateBound(
                new XYZ(area.Xmin / unitFactor.LengthRatio, area.Ymax / unitFactor.LengthRatio, 0),
                new XYZ(area.Xmax / unitFactor.LengthRatio, area.Ymax / unitFactor.LengthRatio, 0)));

            // The normal vector (0,0,1) that must be perpendicular to the profile.
            XYZ normal = XYZ.BasisZ;
            return document.Create.NewFloor(profile, floorType, level, false, normal);
        }
    }
}
