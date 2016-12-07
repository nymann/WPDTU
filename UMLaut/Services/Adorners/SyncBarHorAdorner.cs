using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UMLaut.Resources;
using UMLaut.ViewModel;

namespace UMLaut.Services.Adorners
{
    class SyncBarHorAdorner : Adorner
    {

        Thumb topLeft, topMiddle, topRight, bottomLeft, bottomMiddle, bottomRight;

        VisualCollection visualChildren;

        public SyncBarHorAdorner(UIElement adornerdElement) : base(adornerdElement)
        {
            visualChildren = new VisualCollection(this);

            //Initialize connection points
            BuildConnectionPoint(ref topLeft);
            BuildConnectionPoint(ref topMiddle);
            BuildConnectionPoint(ref topRight);
            BuildConnectionPoint(ref bottomLeft);
            BuildConnectionPoint(ref bottomMiddle);
            BuildConnectionPoint(ref bottomRight);

            // eventhandlers
            topLeft.MouseRightButtonDown += new MouseButtonEventHandler(HandleTopLeft);
            topMiddle.MouseRightButtonDown += new MouseButtonEventHandler(HandleTopMiddle);
            topRight.MouseRightButtonDown += new MouseButtonEventHandler(HandleTopRight);

            bottomLeft.MouseRightButtonDown += new MouseButtonEventHandler(HandleBottomLeft);
            bottomMiddle.MouseRightButtonDown += new MouseButtonEventHandler(HandleBottomMiddle);
            bottomRight.MouseRightButtonDown += new MouseButtonEventHandler(HandleBottomRight);


        }

        void HandleTopLeft(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = -shape.Width / 4;
            shape.OffsetY = -shape.Height / 2;

            SetFocus(ref topLeft);
        }
        void HandleTopMiddle(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = 0;
            shape.OffsetY = -shape.Height / 2;

            SetFocus(ref topMiddle);
        }
        void HandleTopRight(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = shape.Width / 4;
            shape.OffsetY = -shape.Height / 2;

            SetFocus(ref topRight);
        }
        void HandleBottomLeft(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = -shape.Width / 4;
            shape.OffsetY = shape.Height / 2;

            SetFocus(ref bottomLeft);
        }
        void HandleBottomMiddle(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = 0;
            shape.OffsetY = shape.Height / 2;

            SetFocus(ref bottomMiddle);
        }
        void HandleBottomRight(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = shape.Width / 4;
            shape.OffsetY = shape.Height / 2;

            SetFocus(ref bottomRight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            

            topMiddle.Arrange(new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            topLeft.Arrange(new Rect(ActualWidth * 1 / 4 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            topRight.Arrange(new Rect(ActualWidth * 3 / 4 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            bottomMiddle.Arrange(new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
            bottomLeft.Arrange(new Rect(ActualWidth * 1 / 4 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
            bottomRight.Arrange(new Rect(ActualWidth * 3 / 4 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));

            // Return the final size.
            return finalSize;
        }
        void SetFocus(ref Thumb focus)
        {
            topRight.Background = Constants.CornerBoxColor;
            topLeft.Background = Constants.CornerBoxColor;
            topMiddle.Background = Constants.CornerBoxColor;
            bottomLeft.Background = Constants.CornerBoxColor;
            bottomRight.Background = Constants.CornerBoxColor;
            bottomMiddle.Background = Constants.CornerBoxColor;

            focus.Background = Constants.CornerBoxColorFocus;
        }

        void BuildConnectionPoint(ref Thumb point)
        {

            if (point != null) return;

            point = new Thumb();
            point.Height = point.Width = Constants.CornerBoxSize;
            point.Background = Constants.CornerBoxColor;
            point.Opacity = 1;
            point.Cursor = Cursors.Cross;

            visualChildren.Add(point);
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
