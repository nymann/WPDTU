using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UMLaut.Model.Enum;
using UMLaut.Resources;

namespace UMLaut.Model
{
    public class UMLShape :  IShape
    {
        private Guid _id;
        private double _height;
        private double _width;
        private EShape _type;
        private string _label = "";
        private double _x;
        private double _y;

        private UMLShape()
        {

        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="X">Starting X position</param>
        /// <param name="Y">Starting Y position</param>
        /// <param name="Type">Shape type</param>
        public UMLShape(double X, double Y, EShape Type)
        {
            _id = Guid.NewGuid();
            _height = Constants.Drawables.Shapes.DefaultHeight;
            _width = Constants.Drawables.Shapes.DefaultWidth;
            _x = X -  _width/ 2;
            _y = Y - _height / 2;
            _type = Type;
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
            _id = Guid.NewGuid();
            _height = Height;
            _width = Width;
            _x = X - _width / 2;
            _y = Y -  _height/ 2;
            _type = Type;
        }

        /// <summary>
        /// Id of the shape
        /// </summary>
        public Guid Id { get; } = new Guid();
        /// <summary>
        /// X position of the shape
        /// </summary>
        public double X {
            get { return _x; }
            set
            {
                if (_x == value)
                {
                    return;
                }
                _x = value;
            } 
        }
        /// <summary>
        /// Y Position of the shape
        /// </summary>
        public double Y {
            get { return _y; }
            set 
            {
                if (_y == value) {
                    return;
                }
                _y = value;
            }
        }
        /// <summary>
        /// Type of the shape, used for determining the UserControl used for drawing the shape
        /// </summary>
        public EShape Type {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Height of the shape, defaults to 200
        /// </summary>
        public double Height {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// Width of the shape, defualts to 200
        /// </summary>
        public double Width {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// Text of the shape
        /// </summary>
        public string Label {
            get { return _label; }
            set { _label = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
