using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UMLaut.Model.Implementation;
using UMLaut.Serialization;
using UMLaut.Model;
using System.Windows.Controls;

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private Model.Enum.EShape _toolboxValue;

        private readonly Diagram _diagram = new Diagram();
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

            this.IsInitialNode = new RelayCommand<object>(this.PerformIsInitialNode);
            this.IsFinalNode = new RelayCommand<object>(this.PerformIsFinalNode);
            this.IsMergeNode = new RelayCommand<object>(this.PerformIsMergelNode);
            this.IsAction = new RelayCommand<object>(this.PerformIsAction);
            this.IsSyncBarHor = new RelayCommand<object>(this.PerformIsSyncBarHor);
            this.IsSyncBarVert = new RelayCommand<object>(this.PerformIsSyncBarVert);
            this.IsEdge = new RelayCommand<object>(this.PerformIsEdge);
            this.IsTimeEvent = new RelayCommand<object>(this.PerformIsTimeEvent);
            this.IsSendSignal = new RelayCommand<object>(this.PerformIsSendSignal);
            this.IsReceiveSignal = new RelayCommand<object>(this.PerformIsReceiveSignal);

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
        public ICommand CanvasMouseDown { get; set; }
        #endregion


        #region Toolbox ICommands

        public ICommand IsInitialNode { get; set; }
        public ICommand IsFinalNode { get; set; }
        public ICommand IsMergeNode { get; set; }
        public ICommand IsAction { get; set; }
        public ICommand IsSyncBarHor { get; set; }
        public ICommand IsSyncBarVert { get; set; }
        public ICommand IsEdge { get; set; }
        public ICommand IsTimeEvent { get; set; }
        public ICommand IsSendSignal { get; set; }
        public ICommand IsReceiveSignal { get; set; }
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
                if (openFileDialog.ShowDialog() == true)
                {
                    var path = openFileDialog.FileName;
                    deserializer.DeserializeFromFile(path);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Der opstod en fejl.");
            }

        }

        private void PerformSaveFile(object obj)
        {
            Serializer serializer = new Serializer();

            if (String.IsNullOrEmpty(_diagram.FilePath))
            {
                ShowSaveDialogAndSetDiagramFilePath(_diagram);
            }

           serializer.SerializeToFile(_diagram);
        }

        private void PerformSaveFileAs(object obj)
        {
            Serializer serializer = new Serializer();
            ShowSaveDialogAndSetDiagramFilePath(_diagram);
            serializer.SerializeToFile(_diagram);

        }

        private void PerformDuplicateShape(object obj)
        {
            throw new NotImplementedException();
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

        public void PerformFreeHand(object obj)
        {
            _drawingMode = false;
        }

        public void PerformIsInitialNode(object obj)
        {
            _drawingMode = false; // for testing - should be true
            _toolboxValue = Model.Enum.EShape.Initial;
        }


        private void PerformIsFinalNode(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.ActivityFinal;
        }

        private void PerformIsMergelNode(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.Merge;
        }

        private void PerformIsAction(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.Action;
        }

        private void PerformIsSyncBarHor(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.SyncBarHor;
        }

        private void PerformIsSyncBarVert(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.SyncBarVert;
        }

        private void PerformIsEdge(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.Edge;
        }

        private void PerformIsTimeEvent(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.TimeEvent;
        }

        private void PerformIsSendSignal(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.SendSignal;
        }

        private void PerformIsReceiveSignal(object obj)
        {
            _drawingMode = true;
            _toolboxValue = Model.Enum.EShape.ReceiveSignal;

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

                if (_drawingMode)
                {
                    Shapes.Add(new ShapeViewModel(new UMLShape(point.X, point.Y, _toolboxValue)));
                }
                else if(IsElementHit(source))
                {
                    var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                    SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;                                   
                }
                else
                {
                    SelectedElement = null;
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Der opstod en fejl.");
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
        #endregion


        private void ShowSaveDialogAndSetDiagramFilePath(Diagram diagram)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                diagram.FilePath = saveFileDialog.FileName;
            }
        }

        private bool IsElementHit(UIElement source)
        {
            if (source is Canvas || source == null)
                return false;
            return true;
        }
    }


}