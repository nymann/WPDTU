using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using UMLaut.Resources;

namespace UMLaut.Services.Adorners
{
    class BasicAdorner : Adorner
    {
        /*

            source: http://www.nbdtech.com/Blog/archive/2010/06/21/wpf-adorners-part-1-ndash-what-are-adorners.aspx

        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornerdElement"></param>
        public BasicAdorner(UIElement adornerdElement) : base(adornerdElement) { }


        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(0, 0, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(0, ActualHeight - Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth - Constants.CornerBoxSize, 0, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth - Constants.CornerBoxSize, ActualHeight - Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
        }

    }
}
