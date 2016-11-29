using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class PasteCommand : IUndoRedoCommand
    {
        private MainViewModel _mainViewModel;

        public PasteCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            // Undo
            _mainViewModel.StoredElement = _mainViewModel.Shapes.Last();
            _mainViewModel.Shapes.Remove(_mainViewModel.Shapes.Last());
        }

        public void UnExecute()
        {
            // Redo
            _mainViewModel.Shapes.Add(_mainViewModel.StoredElement);
            _mainViewModel.StoredElement = null;
        }
    }
}