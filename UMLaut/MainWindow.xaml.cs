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

namespace UMLaut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UMLDesign _design = new UMLDesign("Test Design");
            _design.addShape(new UMLShape() { Content="Test Shape!", X=20, Y=20 });
            _design.addShape(new UMLShape() { Content = "Test Shape 2!", X = 200, Y = 200 });
            this.DataContext = _design;
        }

        private void UmlCanvas_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
