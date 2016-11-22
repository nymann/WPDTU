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
            
            // TODO(Bug, talk to Kristian).
            var duplicate = _selectedElement;
            duplicate.X = _selectedElement.X + 30;
            duplicate.Y = _selectedElement.Y + 30;
           
            _mainViewModel.Shapes.Add(duplicate);
        }
    }
}