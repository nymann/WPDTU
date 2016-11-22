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
        internal UMLLine Line { get; }

        private ShapeViewModel _to;
        private ShapeViewModel _from;

        public Guid FromId => _from.Id;
        public Guid ToId => _to.Id;

        public ELine Type => Line.Type;

        public LineViewModel(UMLLine line)
        {
            Line = line;
        }

        public ShapeViewModel From {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FromId));
            }
        }

        public ShapeViewModel To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToId));
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
    }
}
