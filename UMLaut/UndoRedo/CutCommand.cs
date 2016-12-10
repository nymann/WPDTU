using System;
using System.Collections.Generic;
using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class CutCommand : IUndoRedoCommand
    {
        private MainViewModel _mainViewModel;
        private List<ShapeViewModel> _storedElement = new List<ShapeViewModel>();

        public CutCommand(MainViewModel mainViewModel, List<ShapeViewModel> storedElement)
        {
            _mainViewModel = mainViewModel;
            _storedElement = storedElement;
        }

        public void Undo()
        {
            // Undo
            foreach (var shape in _storedElement)
            {
                _mainViewModel.Shapes.Add(shape);

            }
            _mainViewModel.StoredElement = new List<ShapeViewModel>();
        }

        public void Redo()
        {
            // Redo

            _mainViewModel.StoredElement = _storedElement;
            foreach (var shape in _storedElement)
            {
                _mainViewModel.Shapes.Remove(shape);
            }

        }
    }
}