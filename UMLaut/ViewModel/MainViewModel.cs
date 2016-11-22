using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Controls;
using UMLaut.Model.Implementation;
using UMLaut.Services;
using UMLaut.Model;
using System.Windows.Controls;
using UMLaut.UndoRedo;
using ICommand = System.Windows.Input.ICommand;
using System.Collections.Generic;
using UMLaut.Resources;
using System.Windows.Documents;
using GalaSoft.MvvmLight.CommandWpf;
using UMLaut.Services.Adorners;
using UMLaut.Model.Enum;

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private EShape _toolboxValue;

        private Diagram _diagram = new Diagram();
        private Point _currentPosition;

        private UndoRedo.UndoRedo undoRedo;

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

        #region Collections

        public ObservableCollection<LineViewModel> Lines { get; set; }
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            this.LaunchNewInstance = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformLaunchNewInstance);
            this.OpenFile = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformOpenFile);
            this.SaveFile = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformSaveFile);
            this.SaveFileAs = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformSaveFileAs);
            this.DuplicateShape = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformDuplicateShape);
            this.Cut = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformCut);
            this.Copy = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformCopy);
            this.Paste = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformPaste);
            this.DeleteShape = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformDeleteShape);
            this.TextToShape = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformTextToShape);
            this.ZoomIn = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformZoomIn);
            this.ZoomOut = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformZoomOut);
            this.ZoomToFit = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformZoomToFit);
            this.Undo = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformUndo);
            this.Redo = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformRedo);


            this.CanvasMouseDown =
                new GalaSoft.MvvmLight.Command.RelayCommand<MouseButtonEventArgs>(this.PerformCanvasMouseDown);
            this.CanvasMouseMove =
                new GalaSoft.MvvmLight.Command.RelayCommand<System.Windows.Input.MouseEventArgs>(
                    this.PerformCanvasMouseMove);

            this.IsInitialNode = new GalaSoft.MvvmLight.Command.RelayCommand<object>(this.PerformIsInitialNode);
            //this.IsFinalNode = new RelayCommand<object>(this.PerformIsFinalNode);
            //this.IsMergeNode = new RelayCommand<object>(this.PerformIsMergelNode);
            //this.IsAction = new RelayCommand<object>(this.PerformIsAction);
            //this.IsSyncBarHor = new RelayCommand<object>(this.PerformIsSyncBarHor);
            //this.IsSyncBarVert = new RelayCommand<object>(this.PerformIsSyncBarVert);
            //this.IsEdge = new RelayCommand<object>(this.PerformIsEdge);
            //this.IsTimeEvent = new RelayCommand<object>(this.PerformIsTimeEvent);
            //this.IsSendSignal = new RelayCommand<object>(this.PerformIsSendSignal);
            //this.IsReceiveSignal = new RelayCommand<object>(this.PerformIsReceiveSignal);

            ShapeToolboxSelection = new GalaSoft.MvvmLight.Command.RelayCommand<EShape>(SetShapeToolboxSelection);
            LineToolboxSelection = new GalaSoft.MvvmLight.Command.RelayCommand<ELine>(SetLineToolboxSelection);

            undoRedo = new UndoRedo.UndoRedo();
        }

        #endregion

        #region ICommands

        #region Ribbon ICommands

        public ICommand LaunchNewInstance { get; set; }
        public ICommand OpenFile { get; set; }
        public ICommand SaveFile { get; set; }
        public ICommand SaveFileAs { get; set; }
        public ICommand DuplicateShape { get; set; }
        public ICommand Cut { get; set; }
        public ICommand Paste { get; set; }
        public ICommand Copy { get; set; }
        public ICommand DeleteShape { get; set; }
        public ICommand TextToShape { get; set; }
        public ICommand ZoomIn { get; set; }
        public ICommand ZoomOut { get; set; }
        public ICommand ZoomToFit { get; set; }

        #endregion

        #region Canvas ICommands

        public ICommand CanvasMouseDown { get; set; }
        public ICommand Undo { get; set; }
        public ICommand Redo { get; set; }
        public ICommand CanvasMouseMove { get; set; }

        #endregion

        #region Toolbox ICommands

        public ICommand ShapeToolboxSelection { get; set; }
        public ICommand LineToolboxSelection { get; set; }

        public ICommand IsInitialNode { get; set; }
        //public ICommand IsFinalNode { get; set; }
        //public ICommand IsMergeNode { get; set; }
        //public ICommand IsAction { get; set; }
        //public ICommand IsSyncBarHor { get; set; }
        //public ICommand IsSyncBarVert { get; set; }
        //public ICommand IsEdge { get; set; }
        //public ICommand IsTimeEvent { get; set; }
        //public ICommand IsSendSignal { get; set; }
        //public ICommand IsReceiveSignal { get; set; }

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
                    UpdateApplicationStateFromDiagram();
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
            }
        }

        private void UpdateApplicationStateFromDiagram()
        {
            foreach (UMLShape umlShape in _diagram.Shapes)
            {
                Shapes.Add(new ShapeViewModel(umlShape));
            }
            foreach (UMLLine umlLine in _diagram.Lines)
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

        private void PerformPaste(object obj)
        {
            if (_storedElement == null) return;
            Shapes.Add(_storedElement);
            _storedElement = null;
            IUndoRedoCommand cmd = new PasteCommand(this);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformDuplicateShape(object obj)
        {
            if (SelectedElement == null) return;
            var duplicate = new ShapeViewModel(SelectedElement.Shape);
            duplicate.X += Constants.DuplicateOffset;
            duplicate.Y += Constants.DuplicateOffset;
            Shapes.Add(duplicate);
            SelectedElement = duplicate;

            IUndoRedoCommand cmd = new DuplicateCommand(duplicate, this);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformDeleteShape(object obj)
        {
            if (Shapes.Count <= 0 || SelectedElement == null) return;
            IUndoRedoCommand cmd = new DeleteCommand(SelectedElement, this);
            Shapes.Remove(SelectedElement);
            undoRedo.InsertInUndoRedo(cmd);
        }

        private void PerformTextToShape(object obj)
        {
            throw new NotImplementedException();
        }

        private void PerformZoomIn(object obj)
        {
            throw new NotImplementedException();
        }

        private void PerformZoomOut(object obj)
        {
            throw new NotImplementedException();
        }

        private void PerformZoomToFit(object obj)
        {
            throw new NotImplementedException();
        }

        #region UndoRedo commands

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

        #endregion

        #endregion

        #region Toolbox commands

        // TODO Should be done in one function insted of split onto two.

        /// <summary>
        /// Set the shape selected in the toolbox
        /// </summary>
        /// <param name="shape"></param>
        public void SetShapeToolboxSelection(EShape shape)
        {
            _drawingMode = true;
            _toolboxValue = shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void SetLineToolboxSelection(ELine line)
        {
            _drawingMode = true;
            //_toolboxValue = line as Enum;
        }

        //public void PerformFreeHand(object obj)
        //{
        //    _drawingMode = false;
        //}

        public void PerformIsInitialNode(object obj)
        {
            _drawingMode = false; // for testing - should be true
            _toolboxValue = Model.Enum.EShape.Initial;
        }


        //private void PerformIsFinalNode(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.ActivityFinal;
        //}

        //private void PerformIsMergelNode(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.Merge;
        //}

        //private void PerformIsAction(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.Action;
        //}

        //private void PerformIsSyncBarHor(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.SyncBarHor;
        //}

        //private void PerformIsSyncBarVert(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.SyncBarVert;
        //}

        //private void PerformIsEdge(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.Edge;
        //}

        //private void PerformIsTimeEvent(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.TimeEvent;
        //}

        //private void PerformIsSendSignal(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.EShape.SendSignal;
        //}

        //private void PerformIsReceiveSignal(object obj)
        //{
        //    _drawingMode = true;
        //    _toolboxValue = Model.Enum.ESfhape.ReceiveSignal;

        //}

        #endregion

        #region Properties commands

        #endregion

        #region Canvas commands

        private void PerformCanvasMouseDown(MouseButtonEventArgs e)
        {
            try
            {
                var source = e.Source as UIElement;
                var point = e.GetPosition(source);

                // TODO: The behavior is kinda fishy..
                if (_drawingMode)
                {
                    var shapeToAdd = new ShapeViewModel(new UMLShape(point.X, point.Y, _toolboxValue));
                    Shapes.Add(shapeToAdd);
                    IUndoRedoCommand cmd = new AddShapeCommand(shapeToAdd, this);
                    undoRedo.InsertInUndoRedo(cmd);

                }
                else if (IsElementHit(source))
                {
                    var shapeVisualElement = (FrameworkElement) e.MouseDevice.Target;
                    SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
                    //AddAdorner(source);
                    //SelectElement(e.MouseDevice.Target, source);                              
                }
                else
                {
                    //RemoveAdorner(source);
                    SelectedElement = null;
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Constants.Messages.GenericError);
                Console.WriteLine(ex.Message);
            }
        }

        private void PerformCanvasMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            var source = e.Source as UIElement;
            CurrentPosition = e.GetPosition(source);
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
            var shapeVisualElement = (FrameworkElement) target;
            SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
            AddAdorner(element);
        }

        private void DeselectElement(UIElement element)
        {
        }

        private bool IsElementHit(UIElement source)
        {
            //if (source is Canvas || source == null)
            //    return false;
            //return true;
            Console.Write(!(source is Canvas));
            Console.Write(!(source is Canvas) || source == null);
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
                AdornerLayer.GetAdornerLayer(element).Remove(adorners[0]);
            }
            catch
            {
            }
        }

        private void UpdateDiagramFromApplicationCurrentState()
        {
            List<UMLLine> umlLines = new List<UMLLine>();

            List<UMLShape> umlShapes = new List<UMLShape>();

            foreach (LineViewModel lvm in Lines)
            {
                umlLines.Add(lvm.Line);
            }
            foreach (ShapeViewModel svm in Shapes)
            {
                umlShapes.Add(svm.Shape);
            }

            _diagram.Lines = umlLines;
            _diagram.Shapes = umlShapes;
        }
    }
}