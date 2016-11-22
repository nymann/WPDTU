﻿using System;
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

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _drawingMode;
        private EShape _toolboxValue;

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
            //this.IsFinalNode = new RelayCommand<object>(this.PerformIsFinalNode);
            //this.IsMergeNode = new RelayCommand<object>(this.PerformIsMergelNode);
            //this.IsAction = new RelayCommand<object>(this.PerformIsAction);
            //this.IsSyncBarHor = new RelayCommand<object>(this.PerformIsSyncBarHor);
            //this.IsSyncBarVert = new RelayCommand<object>(this.PerformIsSyncBarVert);
            //this.IsEdge = new RelayCommand<object>(this.PerformIsEdge);
            //this.IsTimeEvent = new RelayCommand<object>(this.PerformIsTimeEvent);
            //this.IsSendSignal = new RelayCommand<object>(this.PerformIsSendSignal);
            //this.IsReceiveSignal = new RelayCommand<object>(this.PerformIsReceiveSignal);

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
        //    _toolboxValue = Model.Enum.EShape.ReceiveSignal;

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
                    Shapes.Add(new ShapeViewModel(new UMLShape(point.X, point.Y, _toolboxValue)));
                }
                else if(IsElementHit(source))
                {
                    var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                    SelectedElement = shapeVisualElement.DataContext as ShapeViewModel;
                    if (!(SelectedElement == null)) { SelectedElement.IsEditing = false; }
                    //AddAdorner(source);
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