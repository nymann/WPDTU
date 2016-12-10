using System.Collections.Generic;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class CutCommand : IUndoRedoCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly List<ShapeViewModel> _storedElement;

        public CutCommand(MainViewModel mainViewModel, List<ShapeViewModel> storedElement)
        {
            _mainViewModel = mainViewModel;
            _storedElement = storedElement;
        }

        public void Undo()
        {
            foreach (var shape in _storedElement)
                _mainViewModel.Shapes.Add(shape);
            _mainViewModel.StoredElement = new List<ShapeViewModel>();
        }

        public void Redo()
        {
            _mainViewModel.StoredElement = _storedElement;
            foreach (var shape in _storedElement)
                _mainViewModel.Shapes.Remove(shape);
        }
    }
}