using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WoodProject
{
    public static class Constants
    {
        public static readonly ReadOnlyDictionary<string, string> WallParameterMapping
            = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    {"Peso por unidad de largo", "Peso por unidad de largo"},
                    {"Id Panel", "ID Panel Constructivo"},
                    {"Longitud Panel", "Largo Panel"},
                    {"Id Mesa", "N° Mesa de armado"},
                    {"Floor", "Piso"},
                    {"Altura panel", "Altura Panel"},
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
                    {"Posicion Segmento 1", "Posición inicio-fin seg. estructural 1"},
                    {"Posicion Segmento 2", "Posición inicio-fin seg. estructural 2"},
                    {"Posicion Segmento 3", "Posición inicio-fin seg. estructural 3"},
                    {"Posicion Segmento 4", "Posición inicio-fin seg. estructural 4"},
                    {"Posicion Segmento 5", "Posición inicio-fin seg. estructural 5"},
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
        
        public static readonly ReadOnlyDictionary<string, string> AreaParameterMapping
            = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    {"Id Panel Constructivo", "ID Panel Constructivo"},
                    {"Largo del Panel", "Largo"},
                    {"N de la Mesa", "N° Mesa de armado"},
                    {"Piso del Panel", "Piso"},
                    {"Ancho del Panel", "Ancho"},
                    {"Espesor", "Espesor"},
                    {"Cantidad de vigas de borde", "Cantidad de vigas de borde"},
                    {"Patron de clavado interior", "Patrón de clavado interior"},
                    {"Patron de clavado perimetral", "Patrón de clavado perimetral"},
                    {"Grado Estructural", "Grado estructural"}
                }
            );
    }
}
