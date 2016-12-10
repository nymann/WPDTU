using System.Collections.Generic;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class PasteCommand : IUndoRedoCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly List<ShapeViewModel> _shapes;

        public PasteCommand(MainViewModel mainViewModel, List<ShapeViewModel> shapes)
        {
            _mainViewModel = mainViewModel;
            _shapes = shapes;
        }

        public void Undo()
        {
            foreach (var shape in _shapes)
                _mainViewModel.Shapes.Remove(shape);
        }

        public void Redo()
        {
            foreach (var shape in _shapes)
                _mainViewModel.Shapes.Add(shape);
        }
    }
}