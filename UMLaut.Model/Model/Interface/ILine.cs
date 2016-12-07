using System;
using UMLaut.Model.Enum;

namespace UMLaut.Model.Interface
{
    public interface ILine
    {
        Guid From { get; set; }
        Guid To { get; set; }
        string Label { get; }
        ELine Type { get; }
    }
}
