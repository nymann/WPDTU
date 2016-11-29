using System;
using UMLaut.Resources;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DuplicateCommand : IUndoRedoCommand
    {
        private ShapeViewModel _selectedElement;
        private MainViewModel _mainViewModel;

        public DuplicateCommand(ShapeViewModel selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            // Undo
            _mainViewModel.Shapes.Remove(_selectedElement);
        }

        public void UnExecute()
        {
            // Redo
            _mainViewModel.Shapes.Add(_selectedElement);
        }
    }
}