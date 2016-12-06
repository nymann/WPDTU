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

            return dir.Equals("X") ? shape.X + shape.OffsetX + shape.Width / 2 : shape.Y + shape.OffsetY + shape.Height / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
