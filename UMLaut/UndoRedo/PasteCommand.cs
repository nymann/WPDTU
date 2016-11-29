using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class PasteCommand : IUndoRedoCommand
    {
        private MainViewModel _mainViewModel;
        private ShapeViewModel _shape;

        public PasteCommand(MainViewModel mainViewModel, ShapeViewModel shape)
        {
            _mainViewModel = mainViewModel;
            _shape = shape;
        }

        public void Execute()
        {
            // Undo
            _mainViewModel.StoredElement = _shape;
            _mainViewModel.Shapes.Remove(_shape);
        }

        public void UnExecute()
        {
            // Redo
            _mainViewModel.Shapes.Add(_shape);
            //_mainViewModel.StoredElement = null;
        }
    }
}