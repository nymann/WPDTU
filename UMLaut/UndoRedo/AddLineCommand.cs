using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class AddLineCommand : IUndoRedoCommand
    {
        private readonly LineViewModel _selectedElement;
        private readonly MainViewModel _mainViewModel;

        public AddLineCommand(LineViewModel selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            // Undo
            _mainViewModel.Lines.Remove(_selectedElement);
        }

        public void UnExecute()
        {
            // Redo
            _mainViewModel.Lines.Add(_selectedElement);
        }
    }
}