using System;
using System.Collections.Generic;
using UMLaut.Resources;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DuplicateCommand : IUndoRedoCommand
    {
        private List<ShapeViewModel> _selectedElement = new List<ShapeViewModel>();
        private MainViewModel _mainViewModel;

        public DuplicateCommand(List<ShapeViewModel> selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Undo()
        {
            // Undo
            foreach (var shape in _selectedElement)
            {
                _mainViewModel.Shapes.Remove(shape);
            }
        }

        public void Redo()
        {
            // Redo
            foreach (var shape in _selectedElement)
            {
                _mainViewModel.Shapes.Add(shape);

            }
        }
    }
}