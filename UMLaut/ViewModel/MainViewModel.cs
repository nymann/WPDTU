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

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private EShape _toolboxShapeValue;
        private ELine _toolboxLineValue;

        private Diagram _diagram = new Diagram();
        private Point _currentPosition;

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

            this.IsInitialNode = new RelayCommand<object>(this.PerformIsInitialNode);

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
        #endregion


        #region Toolbox ICommands
        public ICommand ShapeToolboxSelection { get; set; }
        public ICommand LineToolboxSelection { get; set; }

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
            _toolboxShapeValue = shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void SetLineToolboxSelection(ELine line)
        {
            _drawingMode = true;
            _toolboxLineValue = line;
        }

        //public void PerformFreeHand(object obj)
        //{
        //    _drawingMode = false;
        //}

        public void PerformIsInitialNode(object obj)
        {
            _drawingMode = false; // for testing - should be true
            _toolboxShapeValue = Model.Enum.EShape.Initial;
        }
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
                    var model = new UMLShape(point.X, point.Y, GetDefaultHeight(_toolboxShapeValue), GetDefaultWidth(_toolboxShapeValue), _toolboxShapeValue);
                    Shapes.Add(new ShapeViewModel(model));

                }
                else if(IsElementHit(source))
                {
                    if(SelectedElement != null)
                    {
                        ClearSelection();
                    }
                    SelectedUIElement = source;
                    var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                    SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
                    AddAdorner(source);
                }
                else
                {
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