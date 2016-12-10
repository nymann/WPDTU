using System.Collections.Generic;
using System.Linq;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class PasteCommand : IUndoRedoCommand
    {
        private MainViewModel _mainViewModel;
        private List<ShapeViewModel> _shapes = new List<ShapeViewModel>();

        public PasteCommand(MainViewModel mainViewModel, List<ShapeViewModel> shapes)
        {
            _mainViewModel = mainViewModel;
            _shapes = shapes;
        }

        public void Execute()
        {
            // Undo
            
            foreach (var shape in _shapes)
            {
                _mainViewModel.Shapes.Remove(shape);
            }
        }

        public void UnExecute()
        {
            // Redo
            foreach (var shape in _shapes)
            {
                _mainViewModel.Shapes.Add(shape);

            }
            //_mainViewModel.StoredElement = null;
        }
    }
}