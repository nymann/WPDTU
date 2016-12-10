using System.Collections.Generic;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class CutCommand : IUndoRedoCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly List<ShapeViewModel> _storedElements;
        private readonly List<LineViewModel> _linesToRemove;

        public CutCommand(MainViewModel mainViewModel, List<ShapeViewModel> storedElements, List<LineViewModel> linesToRemove)
        {
            _mainViewModel = mainViewModel;
            _storedElements = storedElements;
            _linesToRemove = linesToRemove;
        }

        public void Undo()
        {
            foreach (var shape in _storedElements)
                _mainViewModel.Shapes.Add(shape);

            foreach (var line in _linesToRemove)
                _mainViewModel.Lines.Add(line);

            _mainViewModel.StoredElements = new List<ShapeViewModel>();

        }

        public void Redo()
        {
            _mainViewModel.StoredElements = _storedElements;
            foreach (var shape in _storedElements)
            {
                _mainViewModel.Shapes.Remove(shape);

                foreach (var lineToRemove in _linesToRemove)
                {
                    _mainViewModel.Lines.Remove(lineToRemove);
                }
            }
        }
    }
}