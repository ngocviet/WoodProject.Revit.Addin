using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WoodProject
{
    public static class Constants
    {
        public static readonly ReadOnlyDictionary<string, string> ParameterMapping
            = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    {"Peso por unidad de largo", "Peso por unidad de largo"},
                    {"Id Panel", "ID Panel Constructivo"},
                    {"Longitud Panel", "Longitud Panel"},
                    {"Id Mesa", "N° de la Mesa"},
                    {"Floor", "Piso del Panel"},
                    {"Altura panel", "Altura del Panel"},
                    {"Tipo Anclaje 1", "Especificación Anclaje seg. estructural 1"},
                    {"Tipo Anclaje 2", "Especificación Anclaje seg. estructural 2"},
                    {"Tipo Anclaje 3", "Especificación Anclaje seg. estructural 3"},
                    {"Tipo Anclaje 4", "Especificación Anclaje seg. estructural 4"},
                    {"Tipo Anclaje 5", "Especificación Anclaje seg. estructural 5"},
                    {"Posicion Anclaje 1", "Posición Anclaje seg. Estructural 1"},
                    {"Posicion Anclaje 2", "Posición Anclaje seg. Estructural 2"},
                    {"Posicion Anclaje 3", "Posición Anclaje seg. Estructural 3"},
                    {"Posicion Anclaje 4", "Posición Anclaje seg. Estructural 4"},
                    {"Posicion Anclaje 5", "Posición Anclaje seg. Estructural 5"},
                    {"qPPDD borde 1", "Cantidad pies derechos de borde seg. 1"},
                    {"qPPDD borde 2", "Cantidad pies derechos de borde seg. 2"},
                    {"qPPDD borde 3", "Cantidad pies derechos de borde seg. 3"},
                    {"qPPDD borde 4", "Cantidad pies derechos de borde seg. 4"},
                    {"qPPDD borde 5", "Cantidad pies derechos de borde seg. 5"},
                    {"Clavado Interior Lateral", "Patrón de clavado interior seg. estructurales laterales"},
                    {"Clavado Perimetral Lateral", "Patrón de clavado perimetral seg. estructurales laterales"},
                    {"Clavado Interior Gravitacional", "Patrón de clavado interior seg. estructurales gravitacionales"},
                    {"Clavado Perimetral Gravitacional", "Patrón de clavado perimetral seg. estructurales gravitacionales"},
                    {"Grado Estructural", "Grado estructural"}
                }
            );
    }
}
