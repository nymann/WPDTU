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

        public void Execute()
        {
            // undo
            _mainViewModel.Shapes.Remove(_selectedElement);
        }

        public void UnExecute()
        {
            // redo
            _mainViewModel.Shapes.Add(_selectedElement);
        }
    }
}