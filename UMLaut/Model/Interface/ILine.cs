using System;
using UMLaut.Model.Enum;
using UMLaut.ViewModel;

namespace UMLaut.Model.Interface
{
    public interface ILine
    {
        ShapeViewModel From { get; set; }
        ShapeViewModel To { get; set; }
        string Label { get; }
        ELine Type { get; }
    }
}
