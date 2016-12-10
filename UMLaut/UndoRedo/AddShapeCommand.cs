using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class AddShapeCommand : IUndoRedoCommand
    {
        private readonly ShapeViewModel _selectedElement;
        private readonly MainViewModel _mainViewModel;

        public AddShapeCommand(ShapeViewModel selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Undo()
        {
            _mainViewModel.Shapes.Remove(_mainViewModel.Shapes.Last());
        }

        public void Redo()
        {
            _mainViewModel.Shapes.Add(_selectedElement);
        }
    }
}