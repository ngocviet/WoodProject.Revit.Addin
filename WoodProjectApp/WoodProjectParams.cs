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
      [JsonProperty(PropertyName = "x")]
      public double X { get; set; } = 0.0;
      [JsonProperty(PropertyName = "y")]
      public double Y { get; set; } = 0.0;
      [JsonProperty(PropertyName = "z")]
      public double Z { get; set; } = 0.0;
   }
   internal class WallLine
   {
      [JsonProperty(PropertyName = "start")]
      public Point Start { get; set; }
      [JsonProperty(PropertyName = "end")]
      public Point End { get; set; }
   }

    internal class WoodProjectItem
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Sx")]
        public double Sx { get; set; }

        [JsonProperty(PropertyName = "Sy")]
        public double Sy { get; set; }

        [JsonProperty(PropertyName = "Ex")]
        public double Ex { get; set; }

        [JsonProperty(PropertyName = "Ey")]
        public double Ey { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "Wall_Type")]
        public string WallType { get; set; }

        [JsonProperty(PropertyName = "Wall_Face_A")]
        public string WallFaceA { get; set; }

        [JsonProperty(PropertyName = "Wall_Face_B")]
        public string WallFaceB { get; set; }

        [JsonProperty(PropertyName = "Associated_Wall")]
        public string AssociatedWall { get; set; }

        [JsonProperty(PropertyName = "Solution")]
        public int? Solution { get; set; }

        [JsonProperty(PropertyName = "Height")]
        public double? Height { get; set; }
        
        [JsonProperty(PropertyName = "Floor")]
        public int Floor { get; set; }

    }

    internal class LevelInfo
    {
        public ElementId Id { get; set; }

        public string Name { get; set; }

        public List<Curve> Curves { get; set; }

        public double Height { get; set; }

        public double Elevator { get; set; }
    }

    internal class WoodProjectParams
    {
        static public List<WoodProjectItem> Parse(string jsonPath)
        {
            try
            {
                if (!File.Exists(jsonPath))
                    return new List<WoodProjectItem>();

                string jsonContents = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<List<WoodProjectItem>>(jsonContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception happens when parsing the json file: " + ex);
                return null;
            }
        }
    }
}
