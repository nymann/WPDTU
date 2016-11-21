using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using UMLaut.Model;
using UMLaut.Model.Enum;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        private ShapeViewModel _selectedElement;
        private MainViewModel _mainViewModel;

        public DeleteCommand(ShapeViewModel selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            Console.WriteLine(@"Undoing.");
            //Console.WriteLine("\nShapes.Count == {0}", _mainViewModel.Shapes.Count);
            _mainViewModel.Shapes.Add(_selectedElement);
            //Console.WriteLine("Shapes.Count == {0}\n", _mainViewModel.Shapes.Count);
        }

        public void UnExecute()
        {
            Console.WriteLine(@"Redoing.");
            _mainViewModel.Shapes.Remove(_selectedElement);
        }
    }
}