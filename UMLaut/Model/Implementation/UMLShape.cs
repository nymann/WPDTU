using System;
using UMLaut.Model.Enum;

namespace UMLaut.Model
{
    class UMLShape : IShape
    {
        /// <summary>
        /// Id of the shape
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// X position of the shape
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y Position of the shape
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Type of the shape, used for determining the UserControl used for drawing the shape
        /// </summary>
        public EShape Type { get; set; }
        /// <summary>
        /// Height of the shape, defaults to 200
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Width of the shape, defualts to 200
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Text of the shape
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="X">Starting X position</param>
        /// <param name="Y">Starting Y position</param>
        /// <param name="Type">Shape type</param>
        public UMLShape(double X, double Y, EShape Type)
        {
            Id = Guid.NewGuid();
            Height = 200;
            Width = 200;
            this.X = X;
            this.Y = Y;
            this.Type = Type;
            Label = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        /// <param name="Type"></param>
        public UMLShape(double X, double Y, double Height, double Width, EShape Type)
        {
            Id = Guid.NewGuid();
            this.Height = Height;
            this.Width = Width;
            this.X = X;
            this.Y = Y;
            this.Type = Type;
            Label = "";
        }

    }
}
