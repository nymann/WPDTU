using System.Collections.Generic;
using UMLaut.Model.Enum;

namespace UMLaut.Model
{
    public interface IShape
    {
        double X { get; }
        double Y { get; }
        EShape Type { get; }
        double Height { get; set; }
        double Width { get; }
    }
}