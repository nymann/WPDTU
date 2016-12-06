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
    class SyncBarHorAdorner : Adorner
    {

        /*

            source: http://www.nbdtech.com/Blog/archive/2010/06/21/wpf-adorners-part-1-ndash-what-are-adorners.aspx

        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornerdElement"></param>
        public SyncBarHorAdorner(UIElement adornerdElement) : base(adornerdElement) { }


        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth * 1 / 4 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth * 3 / 4 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth * 1 / 4 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
            drawingContext.DrawRectangle(Constants.CornerBoxColor, null, new Rect(ActualWidth * 3 / 4 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
        }

    }
}
