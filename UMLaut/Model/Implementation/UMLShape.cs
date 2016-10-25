using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model.Enum;

namespace UMLaut.Model
{
    class UMLShape : IShape
    {
        public double X { get; }
        public double Y { get; }
        public EShape Type { get; }
        public double Height { get; set; }
        public double Width { get; }
    }
}
