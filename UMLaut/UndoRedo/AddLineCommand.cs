using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class AddLineCommand : IUndoRedoCommand
    {
        private LineViewModel _line;
        private MainViewModel _mainViewModel;

        public AddLineCommand(LineViewModel line, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _line = line;
        }

        public void Execute()
        {
            // undo
            _mainViewModel.Lines.Remove(_line);
        }

        public void UnExecute()
        {
            // redo
            _mainViewModel.Lines.Add(_line);
        }
    }
}