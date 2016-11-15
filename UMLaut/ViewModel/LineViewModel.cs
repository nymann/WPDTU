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

        private UMLLine _line;

        public LineViewModel(UMLLine line)
        {
            _line = line;
        }

        public ELine Type => _line.Type;

    }
}
