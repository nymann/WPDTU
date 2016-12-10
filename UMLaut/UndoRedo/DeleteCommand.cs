using System.Collections.Generic;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        private readonly List<ShapeViewModel> _selectedElement;
        private readonly MainViewModel _mainViewModel;

        public DeleteCommand(List<ShapeViewModel> selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Undo()
        {
            foreach (var shape in _selectedElement)
                _mainViewModel.Shapes.Add(shape);
        }

        public void Redo()
        {
            foreach (var shape in _selectedElement)
                _mainViewModel.Shapes.Remove(shape);
        }
    }
}