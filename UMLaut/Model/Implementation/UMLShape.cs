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
        public double X { get; set; }
        public double Y { get; set; }
        public EShape Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string Content { get; set; }
    }
}
