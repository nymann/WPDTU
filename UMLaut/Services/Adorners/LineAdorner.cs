using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UMLaut.Resources;

namespace UMLaut.Services.Adorners
{

    /// <summary>
    /// Based on 
    /// https://denisvuyka.wordpress.com/2007/10/15/wpf-simple-adorner-usage-with-drag-and-resize-operations/
    /// </summary>

    class LineAdorner : Adorner
    {
        // Draggable thumbs for the line
        Thumb lineStart, lineEnd;

        VisualCollection visualChildren;

        public LineAdorner(UIElement adornedElement) : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);

            // Add the two thumbs
            AddThumbCorner(ref lineStart, null);
            AddThumbCorner(ref lineEnd, null);

        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            lineStart.Arrange(new Rect(0, 0, adornerWidth, adornerHeight));
            lineEnd.Arrange(new Rect(0, adornerWidth, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        private void AddThumbCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb();

            cornerThumb.Height = cornerThumb.Width = Constants.CornerBoxSize;
            cornerThumb.Opacity = 0.40;
            cornerThumb.Background = new SolidColorBrush(Colors.MediumBlue);

            visualChildren.Add(cornerThumb);

        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
