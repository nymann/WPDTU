using System;
using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class CutCommand : IUndoRedoCommand
    {
        private MainViewModel _mainViewModel;
        private ShapeViewModel _selectedElement;

        public CutCommand(MainViewModel mainViewModel, ShapeViewModel selectedElement)
        {
            _mainViewModel = mainViewModel;
            _selectedElement = selectedElement;
        }

        public void Execute()
        {
            // Undo
            _mainViewModel.Shapes.Add(_mainViewModel.StoredElement);
            _mainViewModel.StoredElement = null;
        }

        public void UnExecute()
        {
            // Redo
            _mainViewModel.StoredElement = _selectedElement;
            _mainViewModel.Shapes.Remove(_selectedElement);

        }
    }
}