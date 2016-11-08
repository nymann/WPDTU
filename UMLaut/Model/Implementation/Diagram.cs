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

        private static Diagram instance = null;

        private Diagram()
        {

        }

        public static Diagram Instance
        {
            get
            {
                if (instance == null)
                    instance = new Diagram();
                return instance;
            }
        }

        public List<IShape> Shapes { get; set; }
        public List<ILine> Lines { get; set; }
        public String FilePath { get; set; }
    }
}
