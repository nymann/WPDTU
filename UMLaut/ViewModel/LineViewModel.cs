using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model.Enum;
using UMLaut.Model.Implementation;

namespace UMLaut.ViewModel
{
    public class LineViewModel : BaseViewModel
    {
        internal UMLLine Line { get; set; }

        //private ShapeViewModel _to;
        //private ShapeViewModel _from;

        //public ShapeViewModel FromId => _from;
        //public ShapeViewModel ToId => _to;

        public ELine Type => Line.Type;

        public LineViewModel(UMLLine line)
        {
            Line = line;
        }

        public ShapeViewModel From {
            get
            {
                return Line.From;
            }
            set
            {
                Line.From = value;
                OnPropertyChanged();
            }
        }


        public ShapeViewModel To
        {
            get
            {
                return Line.To;
            }
            set
            {
                Line.To = value;
                OnPropertyChanged();
            }
        }

        public string Label {
            get
            {
                return Line.Label;
            }
            set
            {
                Line.Label = value;
                OnPropertyChanged();
            }


        }

        private double _fromOffsetX, _fromOffsetY, _toOffsetX, _toOffsetY;

        public double FromOffsetX
        {
            get { return _fromOffsetX; }
            set
            {
                _fromOffsetX = value;
                OnPropertyChanged();
            }
        }

        public double FromOffsetY
        {
            get { return _fromOffsetY; }
            set
            {
                _fromOffsetY = value;
                OnPropertyChanged();
            }
        }

        public double ToOffsetX
        {
            get { return _toOffsetX; }
            set
            {
                _toOffsetX = value;
                OnPropertyChanged();
            }
        }

        public double ToOffsetY
        {
            get { return _toOffsetY; }
            set
            {
                _toOffsetY = value;
                OnPropertyChanged();
            }
        }
    }
}
