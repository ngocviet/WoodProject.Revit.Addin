using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace WoodProject
{

    internal class Point
    {
        [JsonProperty(PropertyName = "x")] public double X { get; set; } = 0.0;
        [JsonProperty(PropertyName = "y")] public double Y { get; set; } = 0.0;
        [JsonProperty(PropertyName = "z")] public double Z { get; set; } = 0.0;
    }

    internal class WallLine
    {
        [JsonProperty(PropertyName = "start")] public Point Start { get; set; }
        [JsonProperty(PropertyName = "end")] public Point End { get; set; }
    }

    internal class WoodProjectItem
    {
        [JsonProperty(PropertyName = "solution")]
        public List<Solution> Solutions { get; set; }

        [JsonProperty(PropertyName = "losa")]
        public List<Area> Areas { get; set; }
    }

    internal class Solution
    {
        [JsonProperty(PropertyName = "Id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "Sx")] public double Sx { get; set; }

        [JsonProperty(PropertyName = "Sy")] public double Sy { get; set; }

        [JsonProperty(PropertyName = "Ex")] public double Ex { get; set; }

        [JsonProperty(PropertyName = "Ey")] public double Ey { get; set; }

        [JsonProperty(PropertyName = "Type")] public string Type { get; set; }

        [JsonProperty(PropertyName = "Wall_Type")]
        public string WallType { get; set; }

        [JsonProperty(PropertyName = "Wall_Face_A")]
        public string WallFaceA { get; set; }

        [JsonProperty(PropertyName = "Wall_Face_B")]
        public string WallFaceB { get; set; }

        [JsonProperty(PropertyName = "Associated_Wall")]
        public string AssociatedWall { get; set; }

        [JsonProperty(PropertyName = "Solution")]
        public string SolutionName { get; set; }

        [JsonProperty(PropertyName = "Height")]
        public double? Height { get; set; }

        [JsonProperty(PropertyName = "Floor")] public int Floor { get; set; }
    }

    internal class Area
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "xmax")]
        public double Xmax { get; set; }

        [JsonProperty(PropertyName = "xmin")]
        public double Xmin { get; set; }

        [JsonProperty(PropertyName = "ymax")]
        public double Ymax { get; set; }

        [JsonProperty(PropertyName = "ymin")]
        public double Ymin { get; set; }
    }

    internal class WallInfo
    {
        public Curve Curve { get; set; }

        public ElementId TypeId { get; set; }

        public List<Symbol> WallSymbols { get; set; }
    }
    
    internal class Symbol
    {
        public string Type { get; set; }

        public XYZ StartPoint { get; set; }
    }

    internal class LevelInfo
    {
        public ElementId Id { get; set; }

        public string Name { get; set; }

        public List<WallInfo> WallInfos { get; set; }

        public double Height { get; set; }

        public double Elevator { get; set; }
    }

    internal class WoodProjectParams
    {
        static public WoodProjectItem Parse(string jsonPath)
        {
            try
            {
                if (!File.Exists(jsonPath))
                    return new WoodProjectItem ();

                System.Console.WriteLine(jsonPath);
                string jsonContents = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<WoodProjectItem>(jsonContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception happens when parsing the json file: " + ex);
                return null;
            }
        }
    }
}
