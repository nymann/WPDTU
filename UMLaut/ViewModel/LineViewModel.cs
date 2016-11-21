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

        public LineViewModel(UMLLine line)
        {
            Line = line;
        }

        public Guid FromId {
            get
            {
                return Line.FromId;
            }
            set
            {
                Line.FromId = value;
                OnPropertyChanged();
            }
        }

        public Guid ToId
        {
            get
            {
                return Line.ToId;
            }
            set
            {
                Line.ToId = value;
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
 
        public ELine Type {
            get {
                return Line.Type;
            }
        }

    }
}
