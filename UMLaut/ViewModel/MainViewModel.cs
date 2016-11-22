using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Controls;
using UMLaut.Model.Implementation;
using UMLaut.Services;
using UMLaut.Model;
using System.Collections.Generic;
using UMLaut.Resources;
using System.Windows.Documents;
using UMLaut.Services.Adorners;
using UMLaut.Model.Enum;
using System.Linq;
using System.Windows.Media;

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private EShape _toolboxShapeValue;
        private ELine _toolboxLineValue;

        private Diagram _diagram = new Diagram();
        private Point _currentPosition;

        private Point _beforeMovePosition;

        private ShapeViewModel _selectedElement;
        public ShapeViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                _selectedElement = value;
                OnPropertyChanged();
            }
        }

        public Point CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
                OnPropertyChanged();
            }
        }

        private double _zoomPercentage = 1;
        public double ZoomPercentage
        {
            get { return _zoomPercentage; }
            set
            {
                _zoomPercentage = value;
                OnPropertyChanged();
            }
        }
        private UIElement SelectedUIElement { get; set; }
        #region Collections
        public ObservableCollection<LineViewModel> Lines {get; set;}
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }
        #endregion
    
        #region Constructor

        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            this.LaunchNewInstance = new RelayCommand<object>(this.PerformLaunchNewInstance);
            this.OpenFile = new RelayCommand<object>(this.PerformOpenFile);
            this.SaveFile = new RelayCommand<object>(this.PerformSaveFile);
            this.SaveFileAs = new RelayCommand<object>(this.PerformSaveFileAs);
            this.DuplicateShape = new RelayCommand<object>(this.PerformDuplicateShape);
            this.DeleteShape = new RelayCommand<object>(this.PerformDeleteShape);
            this.TextToShape = new RelayCommand<object>(this.PerformTextToShape);
            this.ZoomIn = new RelayCommand<object>(this.PerformZoomIn);
            this.ZoomOut = new RelayCommand<object>(this.PerformZoomOut);
            this.ZoomToFit = new RelayCommand<object>(this.PerformZoomToFit);

            this.CanvasMouseDown = new RelayCommand<MouseButtonEventArgs>(this.PerformCanvasMouseDown);
            this.CanvasMouseMove = new RelayCommand<System.Windows.Input.MouseEventArgs>(this.PerformCanvasMouseMove);
            this.CanvasMouseWheel = new RelayCommand<MouseWheelEventArgs>(this.PerformCanvasMouseWheel);

            ShapeMouseDown = new RelayCommand<MouseButtonEventArgs>(PerformShapeMouseDown);
            ShapeMove = new RelayCommand<System.Windows.Input.MouseEventArgs>(PerformShapeMove);

            this.IsFreeHand = new RelayCommand<object>(this.PerformIsFreeHand);

            ShapeToolboxSelection = new RelayCommand<EShape>(SetShapeToolboxSelection);
            LineToolboxSelection = new RelayCommand<ELine>(SetLineToolboxSelection);


        }
        #endregion

        #region ICommands

        #region Ribbon ICommands
        public ICommand LaunchNewInstance { get; set; }
        public ICommand OpenFile { get; set; }
        public ICommand SaveFile { get; set; }
        public ICommand SaveFileAs { get; set; }
        public ICommand DuplicateShape { get; set; }
        public ICommand DeleteShape { get; set; }
        public ICommand TextToShape { get; set; }
        public ICommand ZoomIn { get; set; }
        public ICommand ZoomOut { get; set; }
        public ICommand ZoomToFit { get; set; }
        #endregion

        #region Canvas ICommands
        public ICommand CanvasMouseDown { get; set; }
        public ICommand CanvasMouseMove { get; set; }
        public ICommand CanvasMouseWheel { get; set; }

        public ICommand ShapeMouseDown { get; set; }
        public ICommand ShapeMove { get; set; }
        #endregion


        #region Toolbox ICommands
        public ICommand ShapeToolboxSelection { get; set; }
        public ICommand LineToolboxSelection { get; set; }

        public ICommand IsFreeHand { get; set; }
        public ICommand IsInitialNode { get; set; }

        #endregion

        #endregion

        #region Commands
        #region Ribbon commands

        private void PerformLaunchNewInstance(object obj)
        {
            var app = new System.Diagnostics.ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process.Start(app);
        }

        private void PerformOpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Deserializer deserializer = new Deserializer();

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var path = openFileDialog.FileName;
                    _diagram = deserializer.DeserializeFromFile(path);
                    ResetApplicationState();
                    UpdateApplicationStateFromDiagram();
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
            }

        }

        private void ResetApplicationState()
        {
            Lines.Clear();
            Shapes.Clear();
        }

        private void UpdateApplicationStateFromDiagram()
        {
            foreach(UMLShape umlShape in _diagram.Shapes)
            {
                Shapes.Add(new ShapeViewModel(umlShape));
            }
            foreach(UMLLine umlLine in _diagram.Lines)
            {
                Lines.Add(new LineViewModel(umlLine));
            }

        }

        private void PerformSaveFile(object obj)
        {
            try
            {
                Serializer serializer = new Serializer();

                if (String.IsNullOrEmpty(_diagram.FilePath))
                {
                    if (ShowSaveDialogAndSetDiagramFilePath(_diagram))
                    {
                        UpdateDiagramFromApplicationCurrentState();
                        serializer.SerializeToFile(_diagram);
                    }
                }
                else
                {
                    UpdateDiagramFromApplicationCurrentState();
                    serializer.SerializeToFile(_diagram);
                }
            }
           catch (Exception)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
            }
        }


        private void PerformSaveFileAs(object obj)
        {
            Serializer serializer = new Serializer();
            if (ShowSaveDialogAndSetDiagramFilePath(_diagram))
            {
                serializer.SerializeToFile(_diagram);
            }

        }

        private void PerformDuplicateShape(object obj)
        {
            if(SelectedElement != null)
            {
                var duplicateModel = new UMLShape(SelectedElement.Shape.X, SelectedElement.Shape.Y, SelectedElement.Shape.Height, SelectedElement.Shape.Width, SelectedElement.Shape.Type);
                var duplicate = new ShapeViewModel(duplicateModel);
                duplicate.X += Constants.DuplicateOffset;
                duplicate.Y += Constants.DuplicateOffset;
                Shapes.Add(duplicate);
            }
        }

        private void PerformDeleteShape(object obj)
        {
            throw new NotImplementedException();
        }

        private void PerformTextToShape(object obj)
        {
            if (!(SelectedElement == null)) { SelectedElement.IsEditing = !SelectedElement.IsEditing; }
        }

        private void PerformZoomIn(object obj)
        {
            ZoomPercentage += 0.1;
        }

        private void PerformZoomOut(object obj)
        {
            if (ZoomPercentage > 0.49)
            {
                ZoomPercentage -= 0.1;
            }
        }

        private void PerformZoomToFit(object obj)
        {
            ZoomPercentage = 1.0;
        }
        #endregion

        #region Toolbox commands
    
        // TODO Should be done in one function insted of split onto two.

        public void PerformIsFreeHand(object obj)
        {
            _drawingMode = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        /// <summary>
        /// Set the shape selected in the toolbox
        /// </summary>
        /// <param name="shape"></param>
        public void SetShapeToolboxSelection(EShape shape)
        {
            _drawingMode = true;
            _toolboxShapeValue = shape;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void SetLineToolboxSelection(ELine line)
        {
            _drawingMode = true;
            _toolboxLineValue = line;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion

        #region Properties commands
        #endregion

        #region Canvas commands
        private void PerformShapeMouseDown(MouseButtonEventArgs e)
        {
            if (!_drawingMode)
            {
                FrameworkElement movingElement = (FrameworkElement)e.MouseDevice.Target;
                if (movingElement != null)
                {
                    // Save the position of the mouse incase the event is a move
                    _beforeMovePosition = RelativeMousePosition(e);
                }
            }  
        }

        private void PerformShapeMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_drawingMode)
            {
                // Get the element to move
                FrameworkElement element = (FrameworkElement)e.MouseDevice.Target;
                if (element != null)
                {
                    // Retrieve the view model & canvas                 
                    Canvas canvas = FindParentOfType<Canvas>(element);
                    ShapeViewModel movingShape = element.DataContext as ShapeViewModel;

                    Point canvasPosition = Mouse.GetPosition(canvas);
                    var deltaX = canvasPosition.X - _beforeMovePosition.X;
                    var deltaY = canvasPosition.Y - _beforeMovePosition.Y;

                    if (movingShape.X + deltaX >= 0 && movingShape.X + deltaX <= canvas.RenderSize.Width - movingShape.Width)
                    {
                        
                        movingShape.X += deltaX;
                        _beforeMovePosition.X = canvasPosition.X;
                    }

                    if (movingShape.Y + deltaY >= 0 && movingShape.Y + deltaY <= canvas.RenderSize.Height - movingShape.Height)
                    {
                        movingShape.Y += deltaY;
                        _beforeMovePosition.Y = canvasPosition.Y;
                    }






                    //movingShape.X += deltaX;
                    //movingShape.Y += deltaY;
                    //_beforeMovePosition = canvasPosition;

                    //if (newX > 0 && newX < canvas.RenderSize.Width)
                    //{
                    //    movingShape.X += canvasPosition.X - _beforeMovePosition.X;
                    //    _beforeMovePosition.X = newX;
                    //}

                    //if (newY > 0 && newY < canvas.RenderSize.Height)
                    //{
                    //    movingShape.Y += canvasPosition.Y - _beforeMovePosition.Y;
                    //    _beforeMovePosition.Y = newY;
                    //}


                    // Fetch the new position on the canvas
                    //Point relativePosition = RelativeMousePosition(e);
                    //var deltaX = relativePosition.X - _beforeMovePosition.X;
                    //var deltaY = relativePosition.Y - _beforeMovePosition.Y;
                    //movingShape.X += deltaX;
                    //movingShape.Y += deltaY;
                    //_beforeMovePosition = relativePosition;

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void PerformCanvasMouseDown(MouseButtonEventArgs e)
        {
            try
            {
                var source = e.Source as UIElement;

                // TODO: The behavior is kinda fishy..
                if (_drawingMode)
                {
                    Point point;
                    if (source is Canvas)
                    {
                        point = e.GetPosition(source);
                    }
                    else
                    {
                        point = RelativeMousePosition(e);
                    }

                    var model = new UMLShape(point.X, point.Y, GetDefaultHeight(_toolboxShapeValue), GetDefaultWidth(_toolboxShapeValue), _toolboxShapeValue);
                    Shapes.Add(new ShapeViewModel(model));

                }
                else if (IsElementHit(source))
                {
                    if(SelectedElement != null)
                    {
                        ClearSelection();
                    }
                    SelectedUIElement = source;
                    var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                    SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
                    if (!(SelectedElement == null)) { SelectedElement.IsEditing = false; }
                    AddAdorner(source);
                    //SelectElement(e.MouseDevice.Target, source);       
                    if (e.ClickCount.Equals(2)) // if (doubleclick)
                    {
                        SelectedElement.IsEditing = true;
                    }
                }
                else
                {   if (!(SelectedElement == null)) { SelectedElement.IsEditing = false; }
                    //RemoveAdorner(source);
                    SelectedElement = null;
                    ClearSelection();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(ex.Message);
            }
        }

        private void ClearSelection()
        {
            if(SelectedUIElement != null)
            {
                RemoveAdorner(SelectedUIElement);
            }
            SelectedUIElement = null;
            SelectedElement = null;
        }

        /// <summary>
        /// PerformCanvasMouseMove - finds the position of the mouse on the canvas.
        /// </summary>
        /// <param name="e">Mouse Event</param>
        private void PerformCanvasMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            var source = e.MouseDevice.Target as Canvas;
            if (source != null)
            {
                CurrentPosition = e.GetPosition(source);
            }
            else
            {
                CurrentPosition = RelativeMousePosition(e);
            }
        }
        /// <summary>
        /// RelativeMousePosition - Finds the position of the mouse on the canvas
        /// </summary>
        /// <param name="e">Mouse event</param>
        /// <returns>Point of the mouse relative to the canvas</returns>
        private Point RelativeMousePosition(System.Windows.Input.MouseEventArgs e)
        {
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            return Mouse.GetPosition(canvas);
        }

        private void PerformCanvasMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                if (ZoomPercentage > 10)
                {
                    return;
                }
                ZoomPercentage += 0.1;
            }
            else
            {
                if (ZoomPercentage < 0.49)
                {
                    return;
                }
                ZoomPercentage -= 0.1;
            }
        }
        /// <summary>
        /// FindParentOfType - Finds the parent of the passed object recursivly
        /// </summary>
        /// <typeparam name="T">Type of parent to find</typeparam>
        /// <param name="o">Object to look for parent from</param>
        /// <returns>Parent Object</returns>
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
        #endregion
        #endregion


        private bool ShowSaveDialogAndSetDiagramFilePath(Diagram diagram)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "*";
            saveFileDialog.DefaultExt = "ult";
            saveFileDialog.Filter = "UMLaut Diagram|*.ult";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                diagram.FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        private void SelectElement(IInputElement target, UIElement element)
        {
            var shapeVisualElement = (FrameworkElement)target;
            SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
            AddAdorner(element);
        }

        private void DeselectElement(UIElement element)
        {
            
        }

        private bool IsElementHit(UIElement source)
        {
            return !(source is Canvas) || source == null;
        }

        private void AddAdorner(UIElement element)
        {
            AdornerLayer.GetAdornerLayer(element).Add(new BasicAdorner(element));
        }

        private void RemoveAdorner(UIElement element)
        {
            try
            {
                Adorner[] adorners = AdornerLayer.GetAdornerLayer(element).GetAdorners(element);
                foreach(Adorner adorner in adorners)
                {
                    AdornerLayer.GetAdornerLayer(element).Remove(adorner);
                }
            } 
            catch(Exception)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
            }

        }

        private void UpdateDiagramFromApplicationCurrentState()
        {
            List<UMLLine> umlLines = Lines.Select(x => x.Line).ToList();
            List<UMLShape> umlShapes = Shapes.Select(x => x.Shape).ToList();

            _diagram.Lines = umlLines;
            _diagram.Shapes = umlShapes;
        }

        private int GetDefaultHeight(EShape _toolboxValue)
        {
            switch (_toolboxValue)
            {
                case EShape.Action:
                    return Constants.Drawables.Shapes.Action.DefaultHeight;
                case EShape.ActivityFinal:
                    return Constants.Drawables.Shapes.ActivityFinal.DefaultHeight;
                case EShape.Decision:
                    return Constants.Drawables.Shapes.Decision.DefaultHeight;
                case EShape.FlowFinal:
                    return Constants.Drawables.Shapes.FlowFinal.DefaultHeight;
                case EShape.Fork:
                    return Constants.Drawables.Shapes.Fork.DefaultHeight;
                case EShape.Initial:
                    return Constants.Drawables.Shapes.Initial.DefaultHeight;
                case EShape.Join:
                    return Constants.Drawables.Shapes.Join.DefaultHeight;
                case EShape.Merge:
                    return Constants.Drawables.Shapes.Merge.DefaultHeight;
                case EShape.ReceiveSignal:
                    return Constants.Drawables.Shapes.ReceiveSignal.DefaultHeight;
                case EShape.SendSignal:
                    return Constants.Drawables.Shapes.SendSignal.DefaultHeight;
                case EShape.SyncBarHor:
                    return Constants.Drawables.Shapes.SyncBarHor.DefaultHeight;
                case EShape.SyncBarVert:
                    return Constants.Drawables.Shapes.SyncBarVert.DefaultHeight;
                case EShape.TimeEvent:
                    return Constants.Drawables.Shapes.TimeEvent.DefaultHeight;
                default:
                    return Constants.Drawables.Shapes.DefaultHeight;
            }
        }
        private int GetDefaultWidth(EShape _toolboxValue)
        {
            switch (_toolboxValue)
            {
                case EShape.Action:
                    return Constants.Drawables.Shapes.Action.DefaultWidth;
                case EShape.ActivityFinal:
                    return Constants.Drawables.Shapes.ActivityFinal.DefaultWidth;
                case EShape.Decision:
                    return Constants.Drawables.Shapes.Decision.DefaultWidth;
                case EShape.FlowFinal:
                    return Constants.Drawables.Shapes.FlowFinal.DefaultWidth;
                case EShape.Fork:
                    return Constants.Drawables.Shapes.Fork.DefaultWidth;
                case EShape.Initial:
                    return Constants.Drawables.Shapes.Initial.DefaultWidth;
                case EShape.Join:
                    return Constants.Drawables.Shapes.Join.DefaultWidth;
                case EShape.Merge:
                    return Constants.Drawables.Shapes.Merge.DefaultWidth;
                case EShape.ReceiveSignal:
                    return Constants.Drawables.Shapes.ReceiveSignal.DefaultWidth;
                case EShape.SendSignal:
                    return Constants.Drawables.Shapes.SendSignal.DefaultWidth;
                case EShape.SyncBarHor:
                    return Constants.Drawables.Shapes.SyncBarHor.DefaultWidth;
                case EShape.SyncBarVert:
                    return Constants.Drawables.Shapes.SyncBarVert.DefaultWidth;
                case EShape.TimeEvent:
                    return Constants.Drawables.Shapes.TimeEvent.DefaultWidth;
                default:
                    return Constants.Drawables.Shapes.DefaultWidth;
            }
        }
    }


}