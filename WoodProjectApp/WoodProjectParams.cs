﻿using Newtonsoft.Json;
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

    internal class RawWoodProjectItem
    {
        [JsonProperty(PropertyName = "solution")]
        public List<Dictionary<string, string>> Solutions { get; set; }

        [JsonProperty(PropertyName = "losa")]
        public List<Dictionary<string, string>> Areas { get; set; }
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

        [JsonProperty(PropertyName = "Floor")]
        public int Floor { get; set; }

        [JsonProperty(PropertyName = "Id Panel")]
        public int? IdPanel { get; set; }
        
        [JsonProperty(PropertyName = "Peso por unidad de largo")]
        public float? PesoUnidad { get; set; }

        [JsonProperty(PropertyName = "Longitud Panel")]
        public int? Longitud { get; set; }

        [JsonProperty(PropertyName = "Id Mesa")]
        public int? IdMesa { get; set; }

        [JsonProperty(PropertyName = "Altura panel")]
        public int? Altura { get; set; }

        [JsonProperty(PropertyName = "Tipo Anclaje 1")]
        public string TipoAnclaje1 { get; set; }

        [JsonProperty(PropertyName = "Tipo Anclaje 2")]
        public string TipoAnclaje2 { get; set; }

        [JsonProperty(PropertyName = "Tipo Anclaje 3")]
        public string TipoAnclaje3 { get; set; }

        [JsonProperty(PropertyName = "Tipo Anclaje 4")]
        public string TipoAnclaje4 { get; set; }

        [JsonProperty(PropertyName = "Tipo Anclaje 5")]
        public string TipoAnclaje5 { get; set; }

        [JsonProperty(PropertyName = "Posicion Anclaje 1")]
        public string Posicion1 { get; set; }

        [JsonProperty(PropertyName = "Posicion Anclaje 2")]
        public string Posicion2 { get; set; }

        [JsonProperty(PropertyName = "Posicion Anclaje 3")]
        public string Posicion3 { get; set; }

        [JsonProperty(PropertyName = "Posicion Anclaje 4")]
        public string Posicion4 { get; set; }

        [JsonProperty(PropertyName = "Posicion Anclaje 5")]
        public string Posicion5 { get; set; }

        [JsonProperty(PropertyName = "qPPDD borde 1")]
        public string Borde1 { get; set; }

        [JsonProperty(PropertyName = "qPPDD borde 2")]
        public string Borde2 { get; set; }

        [JsonProperty(PropertyName = "qPPDD borde 3")]
        public string Borde3 { get; set; }

        [JsonProperty(PropertyName = "qPPDD borde 4")]
        public string Borde4 { get; set; }

        [JsonProperty(PropertyName = "qPPDD borde 5")]
        public string Borde5 { get; set; }

        [JsonProperty(PropertyName = "Clavado Perimetral Lateral")]
        public int? ClavadoPerimetralLateral { get; set; }

        [JsonProperty(PropertyName = "Clavado Interior Lateral")]
        public int? ClavadoInteriorLateral { get; set; }

        [JsonProperty(PropertyName = "Clavado Perimetral Gravitacional")]
        public int? ClavadoPerimetralGravitacional { get; set; }

        [JsonProperty(PropertyName = "Clavado Interior Gravitacional")]
        public int? ClavadoInteriorGravitacional { get; set; }

        [JsonProperty(PropertyName = "Grado Estructural")]
        public string GradoEstructural { get; set; }
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

        public Solution Solution { get; set; }

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
                    return new WoodProjectItem();

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
