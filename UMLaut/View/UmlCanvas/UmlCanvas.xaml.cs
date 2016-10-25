using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UMLaut.Model;
using UMLaut.Model.Implementation;

namespace UMLaut.View.UmlCanvas
{
    /// <summary>
    /// Interaction logic for UmlCanvas.xaml
    /// </summary>
    public partial class UmlCanvas : UserControl
    {
        public UmlCanvas()
        {
            InitializeComponent();
            List<IShape> shapes = new List<IShape>
            {
                new UMLShape() {X= 20, Y=20, Content="First shape", Height=50, Width=50},
                new UMLShape() {X= 100, Y=50, Content=null, Height=50, Width=50}
            };
            InitializeShapes(shapes);

        }

        /// <summary>
        /// Takes the list of shapes in a given diagram and programitically 
        /// adds each of the to the canvas.
        /// </summary>
        /// <param name="Shapes"></param>
        private void InitializeShapes(List<IShape> shapes)
        {
            foreach(IShape s in shapes)
            {
                // Each shape should already be initialized and should just be added to the canvas
                Button b = new Button { Content = s.Content, Width = s.Width, Height = s.Height };
                Canvas.SetLeft(b, s.X);
                Canvas.SetTop(b, s.Y);
                cnvs.Children.Add(b);
            };
        }
    }
}
