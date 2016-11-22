using System;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DuplicateCommand : IUndoRedoCommand
    {
        private ShapeViewModel _selectedElement;
        private MainViewModel _mainViewModel;

        public DuplicateCommand(ShapeViewModel shapeViewModel, MainViewModel mainViewModel)
        {
            _selectedElement = shapeViewModel;
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            Console.WriteLine(@"Undoing.");
            _mainViewModel.Shapes.Remove(_selectedElement);
        }

        public void UnExecute()
        {
            Console.WriteLine(@"Redoing.");
            _mainViewModel.Shapes.Add(_selectedElement);
        }
    }
}