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

        private Diagram()
        {

        }

        public static Diagram Instance
        { get; }= new Diagram();

        public List<IShape> Shapes { get; set; }
        public List<ILine> Lines { get; set; }
        public String FilePath { get; set; }
    }
}
