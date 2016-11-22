using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model;
using UMLaut.Model.Enum;

namespace UMLaut.ViewModel
{
    public class ShapeViewModel : BaseViewModel, IShape
    {
        private bool _isEditing = false;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }


        internal UMLShape Shape { get; }
        public ShapeViewModel(UMLShape shape)
        {
            Shape = shape;
        }

        Guid IShape.Id
        {
            get { return Shape.Id; }
        }

        public string Label
        {
            get { return Shape.Label; }
            set
            {
                Shape.Label = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return Shape.Height; }
            set
            {
                Shape.Height = value;
                OnPropertyChanged();
            }
        }

        public EShape Type => Shape.Type;

        public double Width
        {
            get { return Shape.Width; }
            set
            {
                Shape.Width = value;
                OnPropertyChanged();
            }
        }

        public double X
        {
            get { return Shape.X; }
            set
            {
                Shape.X = value;
                OnPropertyChanged();
            }
        }
        public double Y
        {
            get { return Shape.Y; }
            set
            {
                Shape.Y = value;
                OnPropertyChanged();
            }
        }

  
    }
}
