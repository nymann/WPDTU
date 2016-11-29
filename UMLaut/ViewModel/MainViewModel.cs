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
using System.Windows.Media.Imaging;
using UMLaut.UndoRedo;
using System.Windows.Controls.Primitives;

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _shapeMode, _lineMode;
        private EShape _toolboxShapeValue;
        private ELine _toolboxLineValue;

        private Diagram _diagram = new Diagram();
        private Point _currentCursorPosition;

        // Points for moving shapes / undo-redo for moved shape
        private Point _undoStartPosition;
        private Point _undoEndPositon = new Point(0, 0);

        private LineViewModel _tempLine;

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

        private ShapeViewModel _storedElement;
        public ShapeViewModel StoredElement
        {
            get { return _storedElement; }
            set
            {
                _storedElement = value;
                OnPropertyChanged();
            }
        }

        public Point CurrentCursorPosition
        {
            get { return _currentCursorPosition; }
            set
            {
                _currentCursorPosition = value;
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

        private UndoRedo.UndoRedo undoRedo;

        #region Collections
        public ObservableCollection<LineViewModel> Lines {get; set;}
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }
        #endregion
    
        #region Constructor

        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            #region Ribbon RelayCommands

            this.LaunchNewInstance = new RelayCommand<object>(this.PerformLaunchNewInstance);
            this.OpenFile = new RelayCommand<object>(this.PerformOpenFile);
            this.SaveFile = new RelayCommand<object>(this.PerformSaveFile);
            this.SaveFileAs = new RelayCommand<object>(this.PerformSaveFileAs);
            this.Paste = new RelayCommand<object>(this.PerformPaste);
            this.Copy = new RelayCommand<object>(this.PerformCopy);
            this.Cut = new RelayCommand<object>(this.PerformCut);
            this.Undo = new RelayCommand<object>(this.PerformUndo);
            this.Redo = new RelayCommand<object>(this.PerformRedo);
            this.DuplicateShape = new RelayCommand<object>(this.PerformDuplicateShape);
            this.DeleteShape = new RelayCommand<object>(this.PerformDeleteShape);
            this.TextToShape = new RelayCommand<object>(this.PerformTextToShape);
            this.ExportDiagram = new RelayCommand<object>(this.PerformExportDiagram);
            this.ZoomIn = new RelayCommand<object>(this.PerformZoomIn);
            this.ZoomOut = new RelayCommand<object>(this.PerformZoomOut);
            this.ZoomToFit = new RelayCommand<object>(this.PerformZoomToFit);

            #endregion


            this.CanvasMouseDown = new RelayCommand<MouseButtonEventArgs>(this.PerformCanvasMouseDown);
            this.CanvasMouseMove = new RelayCommand<System.Windows.Input.MouseEventArgs>(this.PerformCanvasMouseMove);
            this.CanvasMouseWheel = new RelayCommand<MouseWheelEventArgs>(this.PerformCanvasMouseWheel);

            ShapeMouseDown = new RelayCommand<MouseButtonEventArgs>(PerformShapeMouseDown);
            ShapeMouseDoubleClick = new RelayCommand<RoutedEventArgs>(PerformShapeMouseDoubleClick);

            ShapeDragStart = new RelayCommand<DragStartedEventArgs>(PerformShapeDragStart);
            ShapeDrag = new RelayCommand<DragDeltaEventArgs>(PerformShapeDrag);
            ShapeDragEnd = new RelayCommand<DragCompletedEventArgs>(PerformShapeDragEnd);

            this.IsFreeHand = new RelayCommand<object>(this.PerformIsFreeHand);

            ShapeToolboxSelection = new RelayCommand<EShape>(SetShapeToolboxSelection);
            LineToolboxSelection = new RelayCommand<ELine>(SetLineToolboxSelection);

            undoRedo = new UndoRedo.UndoRedo();
        }
        #endregion

        #region ICommands

        #region Ribbon ICommands
        public ICommand LaunchNewInstance { get; set; }
        public ICommand OpenFile { get; set; }
        public ICommand SaveFile { get; set; }
        public ICommand SaveFileAs { get; set; }
        public ICommand Paste { get; set; }
        public ICommand Copy { get; set; }
        public ICommand Cut { get; set; }
        public ICommand Undo { get; set; }
        public ICommand Redo { get; set; }
        public ICommand DuplicateShape { get; set; }
        public ICommand DeleteShape { get; set; }
        public ICommand TextToShape { get; set; }
        public ICommand ExportDiagram { get; set; }

        public ICommand ZoomIn { get; set; }
        public ICommand ZoomOut { get; set; }
        public ICommand ZoomToFit { get; set; }
        #endregion

        #region Canvas ICommands
        public ICommand CanvasMouseDown { get; set; }
        public ICommand CanvasMouseMove { get; set; }
        public ICommand CanvasMouseWheel { get; set; }

        public ICommand ShapeMouseDown { get; set; }
        public ICommand ShapeMouseDoubleClick { get; set; }
        public ICommand ShapeDragStart { get; set; }
        public ICommand ShapeDrag { get; set; }
        public ICommand ShapeDragEnd { get; set; }
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
           catch (Exception e)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void PerformSaveFileAs(object obj)
        {
            try
            {
                Serializer serializer = new Serializer();
                if (ShowSaveDialogAndSetDiagramFilePath(_diagram))
                {
                    serializer.SerializeToFile(_diagram);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(e);
            }


        }

        private void PerformPaste(object obj)
        {
            if (_storedElement == null) return;
            Shapes.Add(_storedElement);
            _storedElement = null;
            IUndoRedoCommand cmd = new PasteCommand(this);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformCopy(object obj)
        {
            if (_selectedElement == null) return;
            _storedElement = _selectedElement;
        }

        private void PerformCut(object obj)
        {
            if (SelectedElement == null) return;
            _storedElement = _selectedElement;
            Shapes.Remove(_selectedElement);
            SelectedElement = null;
            IUndoRedoCommand cmd = new CutCommand(this, _storedElement);
            undoRedo.InsertInUndoRedo(cmd);
        }

        /// <summary>
        /// Currently won't support multi-level undo (int levels = 1)
        /// </summary>
        private void PerformUndo(object obj)
        {
            undoRedo.Undo(1);
        }

        /// <summary>
        /// Currently won't support multi-level redo (int levels = 1)
        /// </summary>
        private void PerformRedo(object obj)
        {
            undoRedo.Redo(1);
        }


        private void PerformDuplicateShape(object obj)
        {
            if (SelectedElement == null) return;

            var duplicateModel = new UMLShape(SelectedElement.Shape.X, SelectedElement.Shape.Y, SelectedElement.Shape.Height, SelectedElement.Shape.Width, SelectedElement.Shape.Type);
            var duplicate = new ShapeViewModel(duplicateModel);
            Shapes.Add(duplicate);

            IUndoRedoCommand cmd = new DuplicateCommand(duplicate, this);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformDeleteShape(object obj)
        {
            if (Shapes.Count <= 0 || _selectedElement == null) return;
            IUndoRedoCommand cmd = new DeleteCommand(_selectedElement, this);
            Shapes.Remove(_selectedElement);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformTextToShape(object obj)
        {
            if (SelectedElement != null)
            {
                SelectedElement.IsEditing = !SelectedElement.IsEditing;
            }
        }
        private void PerformExportDiagram(object parameter)
        {
            var canvas = parameter as Canvas;
            if (canvas != null)
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "*";
                    saveFileDialog.DefaultExt = "png";
                    saveFileDialog.Filter = "Portable Network Graphics|*.png";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var path = saveFileDialog.FileName;
                        RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                              (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                        rtb.Render(canvas);

                        var crop = new CroppedBitmap(rtb, new Int32Rect(50, 50, 250, 250));

                        BitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                        using (var fs = System.IO.File.OpenWrite(path))
                        {
                            pngEncoder.Save(fs);
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                }

            }
            else
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
            }
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
            //_drawingMode = false;
            _shapeMode = _lineMode = false;
            _tempLine = null;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        /// <summary>
        /// Set the shape selected in the toolbox
        /// </summary>
        /// <param name="shape"></param>
        public void SetShapeToolboxSelection(EShape shape)
        {
            //_drawingMode = true;
            _lineMode = false;
            _shapeMode = true;
            _toolboxShapeValue = shape;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void SetLineToolboxSelection(ELine line)
        {
            //_drawingMode = true;
            _lineMode = true;
            _shapeMode = false;
            _toolboxLineValue = line;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion

        #region Properties commands
        #endregion

        #region Canvas commands

        /// <summary>
        /// PerformShapeDragStart - Stores the position of the element in case of an undo command
        /// </summary>
        /// <param name="e">DragStartedEvent</param>
        private void PerformShapeDragStart(DragStartedEventArgs e)
        {
            if (!_shapeMode)
            {
                FrameworkElement element = e.Source as FrameworkElement;
                if (element != null)
                {
                    ShapeViewModel shape = element.DataContext as ShapeViewModel;
                    _undoStartPosition = new Point(shape.X, shape.Y);

                }
            }
        }

        /// <summary>
        /// PerformShapeDrag - Handles the change of the given shapes position.
        /// </summary>
        /// <param name="e"></param>
        private void PerformShapeDrag(DragDeltaEventArgs e)
        {
            if (!_shapeMode)
            {
                //FrameworkElement element = (FrameworkElement)e.Source;
                FrameworkElement element = e.Source as FrameworkElement;
                if (element != null)
                {
                    ShapeViewModel shape = element.DataContext as ShapeViewModel;
                    // Could be kept as an private attr, so the method is only called upon starting the drag?
                    Canvas canvas = FindParentOfType<Canvas>(element);

                    if (shape.X + e.HorizontalChange >= 0 && shape.X + e.HorizontalChange + shape.Width <= canvas.ActualWidth)
                    {
                        shape.X += e.HorizontalChange;
                    }

                    if (shape.Y + e.VerticalChange >= 0 && shape.Y + e.VerticalChange + shape.Height <= canvas.ActualHeight)
                    {
                        shape.Y += e.VerticalChange;
                    }
                }
            }
        }

        /// <summary>
        /// PerformShapeDragEnd - Stores the ending position of the element in case of a undo command
        /// </summary>
        /// <param name="e">DragCompletedEvent</param>
        private void PerformShapeDragEnd(DragCompletedEventArgs e)
        {
            if (!_shapeMode)
            {
                FrameworkElement element = e.Source as FrameworkElement;
                if (element != null)
                {
                    ShapeViewModel shape = element.DataContext as ShapeViewModel;
                    _undoEndPositon = new Point(shape.X, shape.Y);

                    IUndoRedoCommand cmd = new MoveShapeCommand(SelectedElement, _undoStartPosition, _undoEndPositon);

                    undoRedo.InsertInUndoRedo(cmd);
                }
            }
        }

        /// <summary>
        /// PerformShapeMouseDown - Selects the shape associated with the recieved event and 
        /// stores the starting position in-case a drag is initialized.
        /// </summary>
        /// <param name="e">Click event</param>
        private void PerformShapeMouseDown(MouseButtonEventArgs e)
        {
            var source = e.Source as UIElement;
            // Start the line mode.
            if (_lineMode && source != null)
            {
                var fElement = source as FrameworkElement;
                ShapeViewModel shape = fElement.DataContext as ShapeViewModel;
                // End point
                if (_tempLine != null)
                {
                    _tempLine.To = shape;
                    Lines.Add(_tempLine);
                    IUndoRedoCommand cmd = new AddLineCommand(_tempLine, this);
                    undoRedo.InsertInUndoRedo(cmd);
                    _tempLine = null;
                }
                else
                {
                    _tempLine = new LineViewModel(new UMLLine(shape, null, _toolboxLineValue));
                }
                // Skip the event call ladder.
                e.Handled = true;
            }
            // Selection of a shape / drag event.
            else if (!_shapeMode && source != null)
            {
                // Deselect previous
                if (SelectedElement != null)
                {
                    ClearSelection();
                }
                // Select new
                DoSelection(source);
            }  
        }

        /// <summary>
        /// PerformShapeMouseDoubleClick - Double click listener setting the text fields in the shapes to true when 
        /// caught.
        /// </summary>
        /// <param name="e"></param>
        private void PerformShapeMouseDoubleClick(RoutedEventArgs e)
        {
            // Should never be the case because click is called before..
            if (SelectedElement != null)
            {
                SelectedElement.IsEditing = true;
            }
            e.Handled = true;
        }

        /// <summary>
        /// PerformCanvasMouseDown - Called when the canvas is clicked, will NOT be called when clicking a shape.
        /// </summary>
        /// <param name="e"></param>
        private void PerformCanvasMouseDown(MouseButtonEventArgs e)
        {
            try
            {
                // TODO: The behavior is kinda fishy..
                if (_shapeMode)
                {
                    var source = e.Source as UIElement;
                    Point point = source is Canvas ? e.GetPosition(source) : RelativeMousePosition(e);
                    var model = new UMLShape(point.X, point.Y, GetDefaultHeight(_toolboxShapeValue), GetDefaultWidth(_toolboxShapeValue), _toolboxShapeValue);
                    var shapeToAdd = new ShapeViewModel(model);
                    Shapes.Add(shapeToAdd);
                    IUndoRedoCommand cmd = new AddShapeCommand(shapeToAdd, this);
                    undoRedo.InsertInUndoRedo(cmd);
                }
                else if (SelectedElement != null)
                {
                    ClearSelection();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(ex.Message);
            }
        }

        private void DoSelection(UIElement source)
        {
            var fElement = source as FrameworkElement;
            if (fElement != null)
            {
                // Save a reference to the adorned UIElement for removing later
                SelectedUIElement = source;
                SelectedElement = fElement.DataContext as ShapeViewModel;
                AddAdorner(source);
            }
        }

        /// <summary>
        /// ClearSelection - Removes the adorner on the passed UI element
        /// </summary>
        /// <param name="source">Usercontrol as ui element</param>
        private void ClearSelection()
        {
            RemoveAdorner(SelectedUIElement);
            SelectedElement.IsEditing = false;
  
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
                CurrentCursorPosition = e.GetPosition(source);
            }
            else
            {
                CurrentCursorPosition = RelativeMousePosition(e);
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
            //AdornerLayer.GetAdornerLayer(element).Add(new LineAdorner(element));
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
            catch(Exception ex)
            {

                // This isn't a code breaking exception, this would fx happen, if user add some shapes, select one -> delete, and then try to select a new one.
                // Suggested solution is to print the error to the console instead of showing a messagebox.
                //System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(ex.Message);
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