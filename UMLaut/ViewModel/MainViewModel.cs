using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using Microsoft.Win32;

namespace UMLaut.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<LineViewModel> Lines {get; set;}
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }
        public Model.Enum.EShape toolboxValue;


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();

            this.LaunchNewInstance = new RelayCommand<object>(this.PerformLaunchNewInstance);
            this.OpenFile = new RelayCommand<object>(this.PerformOpenFile);
            this.SaveFile = new RelayCommand<object>(this.PerformSaveFile);
            this.DuplicateShape = new RelayCommand<object>(this.PerformDuplicateShape);
            this.DeleteShape = new RelayCommand<object>(this.PerformDeleteShape);
            this.TextToShape = new RelayCommand<object>(this.PerformTextToShape);
            this.ZoomIn = new RelayCommand<object>(this.PerformZoomIn);
            this.ZoomOut = new RelayCommand<object>(this.PerformZoomOut);
            this.ZoomToFit = new RelayCommand<object>(this.PerformZoomToFit);

            /// <summary>
            /// Toolbox buttons
            /// </summary>

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


        public ICommand LaunchNewInstance { get; set; }
        public ICommand OpenFile { get; set; }
        public ICommand SaveFile { get; set; }
        public ICommand DuplicateShape { get; set; }
        public ICommand DeleteShape { get; set; }
        public ICommand TextToShape { get; set; }
        public ICommand ZoomIn { get; set; }
        public ICommand ZoomOut { get; set; }
        public ICommand ZoomToFit { get; set; }

        /// <summary>
        /// Toolbox buttons
        /// </summary>

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


        private void PerformLaunchNewInstance(object obj)
        {
            var app = new System.Diagnostics.ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process.Start(app);
        }

        private void PerformOpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {

            }
        }

        private void PerformSaveFile(object obj)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Toolbox buttons
        /// </summary>

        public void PerformIsInitialNode(object obj)
        {
            toolboxValue = Model.Enum.EShape.InitialNode;
        }


        private void PerformIsFinalNode(object obj)
        {
            toolboxValue = Model.Enum.EShape.FinalNode;
        }

        private void PerformIsMergelNode(object obj)
        {
            toolboxValue = Model.Enum.EShape.MergeNode;
        }

        private void PerformIsAction(object obj)
        {
            toolboxValue = Model.Enum.EShape.Action;
        }

        private void PerformIsSyncBarHor(object obj)
        {
            toolboxValue = Model.Enum.EShape.SyncBarHor;
        }

        private void PerformIsSyncBarVert(object obj)
        {
            toolboxValue = Model.Enum.EShape.SyncBarVert;
        }

        private void PerformIsEdge(object obj)
        {
            toolboxValue = Model.Enum.EShape.Edge;
        }

        private void PerformIsTimeEvent(object obj)
        {
            toolboxValue = Model.Enum.EShape.TimeEvent;
        }

        private void PerformIsSendSignal(object obj)
        {
            toolboxValue = Model.Enum.EShape.SendSignal;
        }

        private void PerformIsReceiveSignal(object obj)
        {
            toolboxValue = Model.Enum.EShape.ReceiveSignal;

        }

    }
}