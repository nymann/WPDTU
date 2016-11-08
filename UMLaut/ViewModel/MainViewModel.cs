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

namespace UMLaut.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Collections
        public ObservableCollection<LineViewModel> Lines {get; set;}
        public ObservableCollection<ShapeViewModel> Shapes { get; set; }
        public CompositeCollection Drawables { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Lines = new ObservableCollection<LineViewModel>();
            Shapes = new ObservableCollection<ShapeViewModel>();
            Drawables = new CompositeCollection(){Shapes, Lines};

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
        }
        #endregion
        
        #region ICommands

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
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Der opstod en fejl.");
            }

        }

        private void PerformSaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Serializer serializer = new Serializer();

            if (Diagram.Instance.FilePath == null)
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    var path = saveFileDialog.FileName;
                    serializer.SerializeToFile(Diagram.Instance, path);
                }
            }
            else
            {
                serializer.SerializeToFile(Diagram.Instance, Diagram.Instance.FilePath);
            }
        }
        
        private void PerformSaveFileAs(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Serializer serializer = new Serializer();

            if (saveFileDialog.ShowDialog() == true)
            {
                var path = saveFileDialog.FileName;
                serializer.SerializeToFile(Diagram.Instance, path);
            }
        }

        private void PerformDuplicateShape(object obj)
        {
            //Lines.Add(new LineViewModel());

            Drawables.Add(new LineViewModel());
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
        #endregion

        #region Properties commands
        #endregion

        #region Canvas commands


        private void PerformCanvasMouseDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                try
                {
                    var point = e.GetPosition((IInputElement)e.Source);
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Der opstod en fejl.");
                }
            }
        }
        #endregion
    }
}