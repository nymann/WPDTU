using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model.Interface;

namespace UMLaut.Model.Implementation
{
    public class Diagram
    {
        public List<UMLShape> Shapes { get; set; }
        public List<UMLLine> Lines { get; set; }
        public String FilePath { get; set; }
    }
}
