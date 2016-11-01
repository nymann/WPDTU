using System;
using UMLaut.Model.Enum;

namespace UMLaut.Model.Interface
{
    interface ILine
    {
        Guid FromId { get; }
        Guid ToId { get; }
        string Label { get; }
        ELine Type { get; }
    }
}
