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

            // Remove the duplicated shape.
            _mainViewModel.Shapes.Remove(_selectedElement);

        }

        public void UnExecute()
        {
            // Redo
            
            // TODO(When duplicating a shape, undo -> redo, then the shape gets added at the top right hand corner despite having the correct X and Y values).

            var duplicate = new ShapeViewModel(_selectedElement.Shape)
            {
                X = Constants.DuplicateOffset,
                Y = Constants.DuplicateOffset
            };

            _mainViewModel.Shapes.Add(duplicate);
        }
    }
}