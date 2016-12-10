using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class AddLineCommand : IUndoRedoCommand
    {
        private readonly LineViewModel _line;
        private readonly MainViewModel _mainViewModel;

        public AddLineCommand(LineViewModel line, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _line = line;
        }

        public void Undo()
        {
            _mainViewModel.Lines.Remove(_line);
        }

        public void Redo()
        {
            _mainViewModel.Lines.Add(_line);
        }
    }
}