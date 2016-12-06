using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UMLaut.ViewModel;

namespace UMLaut.Services.Converter
{
    public class LineMultiConverter : IMultiValueConverter
    {

        /// <summary>
        /// Sort a hackish solution to keep the binding from shape to line
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var dir = parameter as string;
            var shape = values[1] as ShapeViewModel;
            double offsetX = (double)values[2];
            double offsetY = (double)values[3];

            return dir.Equals("X") ? shape.X + offsetX + shape.Width / 2 : shape.Y + offsetY + shape.Height / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
