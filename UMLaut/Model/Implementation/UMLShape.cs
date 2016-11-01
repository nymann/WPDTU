using System;
using UMLaut.Model.Enum;

namespace UMLaut.Model
{
    class UMLShape : IShape
    {
        public Guid Id { get; }
        public double X { get; set; }
        public double Y { get; set; }
        public EShape Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string Label { get; set; }

        public UMLShape(double X, double Y, EShape Type)
        {
            Height = 200;
            Width = 200;
            this.X = X;
            this.Y = Y;
            Id = Guid.NewGuid();
            this.Type = Type;
            Label = "";
        }
    }
}
