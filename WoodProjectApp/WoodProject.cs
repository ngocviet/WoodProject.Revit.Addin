using System;
using System.Collections.Generic;
using System.IO;

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
            List<WoodProjectItem> jsonDeserialized = WoodProjectParams.Parse(filepathJson);

            CreateWalls(jsonDeserialized, doc);

            var saveAsOptions = new SaveAsOptions();
            saveAsOptions.OverwriteExistingFile = true;

            ModelPath path = ModelPathUtils.ConvertUserVisiblePathToModelPath("woodproject_result.rvt");

            doc.SaveAs(path, saveAsOptions);
        }



        private static void CreateWalls(List<WoodProjectItem> jsonDeserialized, Document newDoc)
        {
            FilteredElementCollector levelCollector = new FilteredElementCollector(newDoc);
            levelCollector.OfClass(typeof(Level));
            ElementId someLevelId = levelCollector.FirstElementId();
            if (someLevelId == null || someLevelId.IntegerValue < 0) throw new InvalidDataException("ElementID is invalid.");
            var wallTypeId = newDoc.GetDefaultElementTypeId(ElementTypeGroup.WallType);
            
            UnitConversionFactors unitFactor = new UnitConversionFactors("cm", "N");

            List<LineInfo> LineInfos = new List<LineInfo>();
            foreach (WoodProjectItem item in jsonDeserialized)
            {
                
                XYZ start = new XYZ(item.Sx / unitFactor.LengthRatio, item.Sy / unitFactor.LengthRatio, 0);
                XYZ end = new XYZ(item.Ex / unitFactor.LengthRatio, item.Ey/ unitFactor.LengthRatio, 0);
                var wallHeight = !item.DefaultWallHeight.HasValue || item.DefaultWallHeight < 230 ? 230 :
                    item.DefaultWallHeight > 244 ? 244 :
                    item.DefaultWallHeight.Value;
                LineInfos.Add(new LineInfo
                {
                    Curve = Line.CreateBound(start, end),
                    Height = wallHeight / unitFactor.LengthRatio
                });
            }

            using (Transaction wallTrans = new Transaction(newDoc, "Create some walls"))
            {
                wallTrans.Start();
                var height = 100 / unitFactor.LengthRatio;

                foreach (var lineInfo in LineInfos)
                {
                    Wall.Create(newDoc, lineInfo.Curve, wallTypeId, someLevelId, lineInfo.Height, 0, false,false);
                }

                wallTrans.Commit();
            }
        }
    }
}
