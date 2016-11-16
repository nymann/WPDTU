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
using System.Windows.Controls.Primitives;

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private EShape _toolboxValue;

        private Diagram _diagram = new Diagram();
        private Point _currentPosition;

        public ShapeViewModel TestShape { get; set; }

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
        public ObservableCollection<LineViewModel> Lines {get; set;}
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }
        #endregion
    
        #region Constructor

        public MainViewModel()
        {
            TestShape = new ShapeViewModel(new UMLShape(200, 200, EShape.Action));

            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            Shapes.Add(new ShapeViewModel(new UMLShape(200, 200, EShape.Action)));

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

            ThumbDragCommand = new RelayCommand<DragDeltaEventArgs>(onDragDelta);
            ThumpMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(onMouseDown);

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

        public ICommand ThumbDragCommand { get; set; }
        public ICommand ThumpMouseDownCommand { get; set; }
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
                var duplicate = new ShapeViewModel(SelectedElement.Shape);
                duplicate.X += Constants.DuplicateOffset;
                duplicate.Y += Constants.DuplicateOffset;
                Shapes.Add(duplicate);
                SelectedElement = duplicate;
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
            _toolboxValue = shape;
            //Shapes.Add(new ShapeViewModel(new UMLShape(200, 200, _toolboxValue)));
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

        public void PerformIsInitialNode(object obj)
        {
            _drawingMode = false; // for testing - should be true
            _toolboxValue = Model.Enum.EShape.Initial;
        }
        #endregion

        #region Properties commands
        #endregion

        #region Canvas commands

        public void onMouseDown(MouseButtonEventArgs e)
        {
            Console.Write("Mouse Down!");
        }

        /// <summary>
        /// onDelatDrag - Shape drag event handling. 
        /// </summary>
        /// <param name="e"></param>
        public void onDragDelta(DragDeltaEventArgs e)
        {
            //TODO: Clean this up..
            Thumb source = e.Source as Thumb;

            if (source != null)
            {
                ShapeViewModel movedShape = source.DataContext as ShapeViewModel;
                if (movedShape != null)
                {
                    // Handle the drag
                    movedShape.Shape.X += e.HorizontalChange;
                    movedShape.Shape.Y += e.VerticalChange;
                    OnPropertyChanged();
                }
            }

           

            

        }


        private void PerformCanvasMouseDown(MouseButtonEventArgs e)
        {
            try
            {
                var source = e.Source as UIElement;
                var point = e.GetPosition(source); 

                // TODO: The behavior is kinda fishy..
                if (_drawingMode)
                {
                    Shapes.Add(new ShapeViewModel(new UMLShape(point.X, point.Y, _toolboxValue)));
                }
                else if(IsElementHit(source))
                {
                    var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
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
            var shapeVisualElement = (FrameworkElement)target;
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
            try {
                Adorner[] adorners = AdornerLayer.GetAdornerLayer(element).GetAdorners(element);
                AdornerLayer.GetAdornerLayer(element).Remove(adorners[0]);
           } catch { }

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