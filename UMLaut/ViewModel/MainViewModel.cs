﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
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
        private List<ShapeViewModel> _selectedElements = new List<ShapeViewModel>();
        private List<UIElement> _selectedUiElement = new List<UIElement>();

        public ShapeViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                _selectedElement = value;
                OnPropertyChanged();
            }
        }
        public List<ShapeViewModel> SelectedElements
        {
            get { return _selectedElements; }
            set
            {
                _selectedElements = value;
                OnPropertyChanged();
            }
        }

        public List<UIElement> SelectedUiElement
        {
            get { return _selectedUiElement; }
            set
            {
                _selectedUiElement = value;
                OnPropertyChanged();
            }
        }

        private List<ShapeViewModel> _storedElements = new List<ShapeViewModel>();

        public List<ShapeViewModel> StoredElements
        {
            get { return _storedElements; }
            set
            {
                _storedElements = value;
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

        private bool _zoomOutEnabled = true;

        public bool ZoomOutEnabled
        {
            get { return _zoomOutEnabled; }
            set
            {
                _zoomOutEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _undoEnabled = true;

        public bool UndoEnabled
        {
            get { return _undoEnabled; }
            set
            {
                _undoEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _redoEnabled = true;

        public bool RedoEnabled
        {
            get { return _redoEnabled; }
            set
            {
                _redoEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _duplicateShapeEnabled;

        public bool DuplicateShapeEnabled
        {
            get { return _duplicateShapeEnabled; }
            set
            {
                _duplicateShapeEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _deleteShapeEnabled = false;

        public bool DeleteShapeEnabled
        {
            get { return _deleteShapeEnabled; }
            set
            {
                _deleteShapeEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _textToShapeEnabled = false;

        public bool TextToShapeEnabled
        {
            get { return _textToShapeEnabled; }
            set
            {
                _textToShapeEnabled = value;
                OnPropertyChanged();
            }
        }

        private readonly UndoRedo.UndoRedo _undoRedo;

        #region Collections

        public ObservableCollection<LineViewModel> Lines { get; set; }
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            #region Ribbon RelayCommands

            LaunchNewInstance = new RelayCommand<object>(PerformLaunchNewInstance);
            OpenFile = new RelayCommand<object>(PerformOpenFile);
            SaveFile = new RelayCommand<object>(PerformSaveFile);
            SaveFileAs = new RelayCommand<object>(PerformSaveFileAs);
            Paste = new RelayCommand<object>(PerformPaste);
            Copy = new RelayCommand<object>(PerformCopy);
            Cut = new RelayCommand<object>(PerformCut);
            Undo = new RelayCommand<object>(PerformUndo);
            Redo = new RelayCommand<object>(PerformRedo);
            DuplicateShape = new RelayCommand<object>(PerformDuplicateShape);
            DeleteShape = new RelayCommand<object>(PerformDeleteShape);
            TextToShape = new RelayCommand<object>(PerformTextToShape);
            ExportDiagram = new RelayCommand<object>(PerformExportDiagram);
            ZoomIn = new RelayCommand<object>(PerformZoomIn);
            ZoomOut = new RelayCommand<object>(PerformZoomOut);
            ZoomToFit = new RelayCommand<object>(PerformZoomToFit);

            #endregion

            CanvasMouseDown = new RelayCommand<MouseButtonEventArgs>(PerformCanvasMouseDown);
            CanvasMouseMove = new RelayCommand<System.Windows.Input.MouseEventArgs>(PerformCanvasMouseMove);
            CanvasMouseWheel = new RelayCommand<MouseWheelEventArgs>(PerformCanvasMouseWheel);

            ShapeMouseDown = new RelayCommand<MouseButtonEventArgs>(PerformShapeMouseDown);
            ShapeMouseDoubleClick = new RelayCommand<RoutedEventArgs>(PerformShapeMouseDoubleClick);

            ShapeDragStart = new RelayCommand<DragStartedEventArgs>(PerformShapeDragStart);
            ShapeDrag = new RelayCommand<DragDeltaEventArgs>(PerformShapeDrag);
            ShapeDragEnd = new RelayCommand<DragCompletedEventArgs>(PerformShapeDragEnd);

            IsFreeHand = new RelayCommand<object>(PerformIsFreeHand);

            ShapeToolboxSelection = new RelayCommand<EShape>(SetShapeToolboxSelection);
            LineToolboxSelection = new RelayCommand<ELine>(SetLineToolboxSelection);

            _undoRedo = new UndoRedo.UndoRedo();
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
            var openFileDialog = new OpenFileDialog();
            var deserializer = new Deserializer();

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var path = openFileDialog.FileName;
                    _diagram = deserializer.AsyncDeserializeFromFile(path).Result;
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
            foreach (var umlShape in _diagram.Shapes)
                Shapes.Add(new ShapeViewModel(umlShape));
            foreach (var umlLine in _diagram.Lines)
                Lines.Add(new LineViewModel(umlLine));
        }

        private void PerformSaveFile(object obj)
        {
            try
            {
                var serializer = new Serializer();

                if (string.IsNullOrEmpty(_diagram.FilePath))
                {
                    if (ShowSaveDialogAndSetDiagramFilePath(_diagram))
                    {
                        UpdateDiagramFromApplicationCurrentState();
                        serializer.AsyncSerializeToFile(_diagram);
                    }
                }
                else
                {
                    UpdateDiagramFromApplicationCurrentState();
                    serializer.AsyncSerializeToFile(_diagram);
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
                var serializer = new Serializer();
                if (ShowSaveDialogAndSetDiagramFilePath(_diagram))
                    serializer.AsyncSerializeToFile(_diagram);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(e);
            }
        }

        private void PerformPaste(object obj)
        {
            var temps = new List<ShapeViewModel>();
            var doesShapesContainIntialNode = DoesShapesContainInitialNode();

            if (_storedElements.Count == 0) return;
            foreach (var shape in StoredElements)
            {
                if ((shape.Type == EShape.Initial) && doesShapesContainIntialNode) return;

                var temp =
                    new ShapeViewModel(new UMLShape(shape.X, shape.Y, shape.Height, shape.Width, shape.Shape.Type));
                Shapes.Add(temp);
                temps.Add(temp);
            }
            IUndoRedoCommand cmd = new PasteCommand(this, temps);
            _undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformCopy(object obj)
        {
            if (SelectedElements.Count == 0) return;
            StoredElements = SelectedElements;
        }

        private void PerformCut(object obj)
        {
            if ((Shapes.Count <= 0) || (SelectedElements == null)) return;
            var allLinesToRemove = new List<LineViewModel>();
            StoredElements = SelectedElements;

            foreach (var shape in SelectedElements)
            {
                Shapes.Remove(shape);
                var linesToRemove = Lines.Where(line => (line.ToShape == shape) || (line.FromShape == shape)).ToList();

                foreach (var lineToRemove in linesToRemove)
                {
                    Lines.Remove(lineToRemove);
                    allLinesToRemove.Add(lineToRemove);
                }

            }
            SelectedElements = new List<ShapeViewModel>();
            IUndoRedoCommand cmd = new CutCommand(this, StoredElements, allLinesToRemove);
            _undoRedo.InsertInUndoRedo(cmd);

        }

        /// <summary>
        /// Currently won't support multi-level undo (int levels = 1)
        /// </summary>
        private void PerformUndo(object obj)
        {
            _undoRedo.Undo(1);
        }

        /// <summary>
        /// Currently won't support multi-level redo (int levels = 1)
        /// </summary>
        private void PerformRedo(object obj)
        {
            _undoRedo.Redo(1);
        }


        private void PerformDuplicateShape(object obj)
        {
            if (SelectedElements.Count == 0) return;

            var duplicates = new List<ShapeViewModel>();
            foreach (var shape in SelectedElements)
            {
                var duplicateModel = new UMLShape(shape.Shape.X, shape.Shape.Y, shape.Shape.Height, shape.Shape.Width,
                    shape.Shape.Type);
                var duplicate = new ShapeViewModel(duplicateModel);
                if (duplicate.Type == EShape.Initial) return;
                duplicates.Add(duplicate);
            }
            foreach (var shape in duplicates)
                Shapes.Add(shape);

            IUndoRedoCommand cmd = new DuplicateCommand(duplicates, this);
            _undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformDeleteShape(object obj)
        {
            if ((Shapes.Count <= 0) || (SelectedElements == null)) return;
            var allLinesToRemove = new List<LineViewModel>();

            foreach (var shape in SelectedElements)
            {
                Shapes.Remove(shape);
                var linesToRemove = Lines.Where(line => (line.ToShape == shape) || (line.FromShape == shape)).ToList();

                foreach (var lineToRemove in linesToRemove) { 
                    Lines.Remove(lineToRemove);
                    allLinesToRemove.Add(lineToRemove);
                }

            }

            IUndoRedoCommand cmd = new DeleteCommand(SelectedElements, allLinesToRemove, this);
            _undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformTextToShape(object obj)
        {
            if (SelectedElements[SelectedElements.Count -1] != null)
                SelectedElements[SelectedElements.Count - 1].IsEditing =
                    !SelectedElements[SelectedElements.Count - 1].IsEditing;
        }

        private void PerformExportDiagram(object parameter)
        {
            var canvas = parameter as Canvas;
            if (canvas != null)
                try
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "*";
                    saveFileDialog.DefaultExt = "png";
                    saveFileDialog.Filter = "Portable Network Graphics|*.png";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var path = saveFileDialog.FileName;
                        var rtb = new RenderTargetBitmap((int) canvas.RenderSize.Width,
                            (int) canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
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
            else
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
        }

        private void PerformZoomIn(object obj)
        {
            ZoomPercentage += 0.1;
            validateZoomOutEnabled();
        }

        private void PerformZoomOut(object obj)
        {
            if (ZoomOutEnabled)
                ZoomPercentage -= 0.1;

            validateZoomOutEnabled();
        }

        private void PerformZoomToFit(object obj)
        {
            ZoomPercentage = 1.0;
            validateZoomOutEnabled();
        }

        private void validateZoomOutEnabled()
        {
            if (ZoomPercentage < 0.49)
                ZoomOutEnabled = false;
            else
                ZoomOutEnabled = true;
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
            if (_shapeMode) return;
            var element = e.Source as FrameworkElement;
            if (element == null) return;
            var shape = element.DataContext as ShapeViewModel;
            _undoStartPosition = new Point(shape.X, shape.Y);
        }

        /// <summary>
        /// PerformShapeDrag - Handles the change of the given shapes position.
        /// </summary>
        /// <param name="e"></param>
        private void PerformShapeDrag(DragDeltaEventArgs e)
        {
            if (_shapeMode) return;
            //FrameworkElement element = (FrameworkElement)e.Source;
            var element = e.Source as FrameworkElement;
            if (element == null) return;
            var shape = element.DataContext as ShapeViewModel;
            // Could be kept as an private attr, so the method is only called upon starting the drag?
            var canvas = FindParentOfType<Canvas>(element);

            if ((shape.X + e.HorizontalChange >= 0) &&
                (shape.X + e.HorizontalChange + shape.Width <= canvas.ActualWidth))
                shape.X += e.HorizontalChange;

            if ((shape.Y + e.VerticalChange >= 0) &&
                (shape.Y + e.VerticalChange + shape.Height <= canvas.ActualHeight))
                shape.Y += e.VerticalChange;
        }

        /// <summary>
        /// PerformShapeDragEnd - Stores the ending position of the element in case of a undo command
        /// </summary>
        /// <param name="e">DragCompletedEvent</param>
        private void PerformShapeDragEnd(DragCompletedEventArgs e)
        {
            if (_shapeMode) return;
            var element = e.Source as FrameworkElement;
            if (element == null) return;
            var shape = element.DataContext as ShapeViewModel;
            _undoEndPositon = new Point(shape.X, shape.Y);

            // If we selected a shape, but we didn't move it, then we shouldn't insert it in undoRedo.
            if (!(Math.Abs(_undoEndPositon.X - _undoStartPosition.X) > 1) ||
                !(Math.Abs(_undoEndPositon.Y - _undoStartPosition.Y) > 1)) return;

            IUndoRedoCommand cmd = new MoveShapeCommand(SelectedElements[SelectedElements.Count - 1],
                _undoStartPosition, _undoEndPositon);

            _undoRedo.InsertInUndoRedo(cmd);
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
            if (_lineMode && (source != null))
            {
                var fElement = source as FrameworkElement;
                var shape = fElement.DataContext as ShapeViewModel;
                // End point
                if (_tempLine != null)
                {
                    _tempLine.To = shape.Id;
                    _tempLine.ToShape = shape;
                    _tempLine.ToOffsetX = shape.OffsetX;
                    _tempLine.ToOffsetY = shape.OffsetY;
                    Lines.Add(_tempLine);
                    IUndoRedoCommand cmd = new AddLineCommand(_tempLine, this);
                    _undoRedo.InsertInUndoRedo(cmd);
                    _tempLine = null;
                }
                else
                {
                    _tempLine = new LineViewModel(new UMLLine(shape.Id, _toolboxLineValue));
                    _tempLine.FromShape = shape;
                    _tempLine.FromOffsetX = shape.OffsetX;
                    _tempLine.FromOffsetY = shape.OffsetY;
                }
                // Skip the event call ladder.
                e.Handled = true;
            }
            // Selection of a shape / drag event.
            else if (!_shapeMode && (source != null))
            {
                // Deselect previous
                if (SelectedElements != null)
                {
                    //ClearSelection();
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
            if (SelectedElements.Count > 0)
                SelectedElements[SelectedElements.Count - 1].IsEditing = true;
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
                    var point = source is Canvas ? e.GetPosition(source) : RelativeMousePosition(e);
                    var model = new UMLShape(point.X, point.Y, GetDefaultHeight(_toolboxShapeValue),
                        GetDefaultWidth(_toolboxShapeValue), _toolboxShapeValue);
                    var shapeToAdd = new ShapeViewModel(model);

                    // If we already have a initial node on canvas, don't allow another one.
                    if ((shapeToAdd.Type == EShape.Initial) && DoesShapesContainInitialNode())
                    {
                        System.Windows.MessageBox.Show("Canvas can only contain one initial node.");
                    }
                    else
                    {
                        Shapes.Add(shapeToAdd);
                        IUndoRedoCommand cmd = new AddShapeCommand(shapeToAdd, this);
                        _undoRedo.InsertInUndoRedo(cmd);
                    }
                }
                else if (SelectedElements != null)
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
                SelectedUiElement.Add(source);
                SelectedElements.Add(fElement.DataContext as ShapeViewModel);
                SelectedElement = fElement.DataContext as ShapeViewModel;
                AddAdorner(source);
                setRibbonSelection(true);
            }
        }


        /// <summary>
        /// ClearSelection - Removes the adorner on the passed UI element
        /// </summary>
        /// <param name="source">Usercontrol as ui element</param>
        private void ClearSelection()
        {
            RemoveAdorner(SelectedUiElement);
            foreach (var element in SelectedElements)
                element.IsEditing = false;

            SelectedUiElement = new List<UIElement>();
            SelectedElements = new List<ShapeViewModel>();
            SelectedElement = null;
            setRibbonSelection(false);
        }

        /// <summary>
        /// Set isEnabled of Ribbon buttons
        /// </summary>
        public void setRibbonSelection(bool b)
        {
            DuplicateShapeEnabled = b;
            DeleteShapeEnabled = b;
            TextToShapeEnabled = b;
        }

        /// <summary>
        /// PerformCanvasMouseMove - finds the position of the mouse on the canvas.
        /// </summary>
        /// <param name="e">Mouse Event</param>
        private void PerformCanvasMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            var source = e.MouseDevice.Target as Canvas;
            CurrentCursorPosition = source != null ? e.GetPosition(source) : RelativeMousePosition(e);
        }

        /// <summary>
        /// RelativeMousePosition - Finds the position of the mouse on the canvas
        /// </summary>
        /// <param name="e">Mouse event</param>
        /// <returns>Point of the mouse relative to the canvas</returns>
        private Point RelativeMousePosition(System.Windows.Input.MouseEventArgs e)
        {
            var shapeVisualElement = (FrameworkElement) e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            return Mouse.GetPosition(canvas);
        }

        private void PerformCanvasMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                if (ZoomPercentage > 10)
                    return;
                ZoomPercentage += 0.1;
            }
            else
            {
                if (ZoomPercentage < 0.49)
                    return;
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
            var saveFileDialog = new SaveFileDialog();
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
        
        private void AddAdorner(UIElement element)
        {
            var fElement = element as FrameworkElement;
            //SelectedElements.Add(fElement.DataContext as ShapeViewModel);
            if (SelectedElements[SelectedElements.Count - 1].Type == EShape.SyncBarHor)
            {
                AdornerLayer.GetAdornerLayer(element).Add(new SyncBarHorAdorner(element));
                return;
            }
            else if (SelectedElements[SelectedElements.Count - 1].Type == EShape.Merge)
            {
                AdornerLayer.GetAdornerLayer(element).Add(new MergeAdorner(element));
                return;
            }

            //AdornerLayer.GetAdornerLayer(element).Add(new LineAdorner(element));


            //AdornerLayer.GetAdornerLayer(element).Add(new BasicAdorner(element));
            AdornerLayer.GetAdornerLayer(element).Add(new MergeAdorner(element));
            // temporary, until basic adorner has been expanded
        }

        private void RemoveAdorner(List<UIElement> elements)
        {
            foreach (var element in elements)
                try
                {
                    var adorners = AdornerLayer.GetAdornerLayer(element).GetAdorners(element);
                    foreach (var adorner in adorners)
                        AdornerLayer.GetAdornerLayer(element).Remove(adorner);
                }
                catch (Exception ex)
                {
                    // This isn't a code breaking exception, this would fx happen, if user add some shapes, select one -> delete, and then try to select a new one.
                    // Suggested solution is to print the error to the console instead of showing a messagebox.
                    //System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                    Console.WriteLine(ex.Message);
                }
        }

        private void UpdateDiagramFromApplicationCurrentState()
        {
            var umlLines = Lines.Select(x => x.Line).ToList();
            var umlShapes = Shapes.Select(x => x.Shape).ToList();

            _diagram.Lines = umlLines;
            _diagram.Shapes = umlShapes;
        }

        private int GetDefaultHeight(EShape toolboxValue)
        {
            switch (toolboxValue)
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

        private bool DoesShapesContainInitialNode()
        {
            return Shapes.Any(shape => shape.Type == EShape.Initial);
        }
    }
}