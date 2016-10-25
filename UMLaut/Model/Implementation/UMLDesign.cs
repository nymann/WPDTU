using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLaut.Model.Implementation
{
    class UMLDesign : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public List<IShape> Shapes { get; set; }

        public UMLDesign()
        {
            this.Shapes = new List<IShape>();
        }

        public UMLDesign(string name)
        {
            this.Name = name;
            this.Shapes = new List<IShape>();
        }

        public void addShape(IShape shape)
        {
            this.Shapes.Add(shape);
        }


    }
}
