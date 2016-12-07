using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UMLaut.Resources;
using UMLaut.ViewModel;

namespace UMLaut.Services.Adorners
{
    class MergeAdorner : Adorner
    {

        Thumb Top, Bottom, Left, Right;

        VisualCollection visualChildren;


        public MergeAdorner(UIElement adornerdElement) : base(adornerdElement)
        {
            visualChildren = new VisualCollection(this);

            //Initialized connection points
            BuildConnectionPoint(ref Top);
            BuildConnectionPoint(ref Bottom);
            BuildConnectionPoint(ref Left);
            BuildConnectionPoint(ref Right);

            // Eventhandlers
            Top.MouseRightButtonDown += new MouseButtonEventHandler(HandleTop);
            Bottom.MouseRightButtonDown += new MouseButtonEventHandler(HandleBottom);
            Left.MouseRightButtonDown += new MouseButtonEventHandler(HandleLeft);
            Right.MouseRightButtonDown += new MouseButtonEventHandler(HandleRight);


        }
        void HandleTop(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = 0;
            shape.OffsetY = -shape.Height/2;
            SetFocus(ref Top);

        }
        void HandleBottom(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = 0;
            shape.OffsetY = shape.Height/2;
            SetFocus(ref Bottom);
            
        }
        void HandleLeft(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = -shape.Width / 2;
            shape.OffsetY = 0;
            SetFocus(ref Left);
            
        }
        void HandleRight(object sender, MouseButtonEventArgs args)
        {
            var ele = AdornedElement as FrameworkElement;
            ShapeViewModel shape = ele.DataContext as ShapeViewModel;
            shape.OffsetX = shape.Width / 2;
            shape.OffsetY = 0;
            SetFocus(ref Right);
                    }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Top.Arrange(new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, -Constants.CornerBoxSize, Constants.CornerBoxSize, Constants.CornerBoxSize));
            Bottom.Arrange(new Rect(ActualWidth / 2 - Constants.CornerBoxSize / 2, ActualHeight, Constants.CornerBoxSize, Constants.CornerBoxSize));
            Left.Arrange(new Rect(- Constants.CornerBoxSize, ActualHeight/2 - Constants.CornerBoxSize / 2, Constants.CornerBoxSize, Constants.CornerBoxSize));
            Right.Arrange(new Rect(ActualWidth, ActualHeight / 2 - Constants.CornerBoxSize / 2, Constants.CornerBoxSize, Constants.CornerBoxSize));
            
            // Return the final size.
            return finalSize;
        }

        void SetFocus(ref Thumb focus)
        {
            Right.Background = Constants.CornerBoxColor;
            Left.Background = Constants.CornerBoxColor;
            Top.Background = Constants.CornerBoxColor;
            Bottom.Background = Constants.CornerBoxColor;

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
