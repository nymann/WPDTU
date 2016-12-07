using System;
using System.Collections.Generic;
using UMLaut.Model.Enum;

namespace UMLaut.Model
{
    public interface IShape
    {
        Guid Id { get; }
        double X { get; }
        double Y { get; }
        double Height { get; set; }
        double Width { get; }
        string Label { get; }
        EShape Type { get; }
    }
}