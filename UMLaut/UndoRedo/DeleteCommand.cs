using System.Collections.Generic;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        private readonly List<ShapeViewModel> _selectedElement;
        private readonly MainViewModel _mainViewModel;
        private readonly List<LineViewModel> _removedLines;

        public DeleteCommand(List<ShapeViewModel> selectedElement, List<LineViewModel> removedLines, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
            _removedLines = removedLines;
        }

        public void Undo()
        {
            foreach (var shape in _selectedElement)
                _mainViewModel.Shapes.Add(shape);

            foreach (var removedLine in _removedLines)
                _mainViewModel.Lines.Add(removedLine);
        }

        public void Redo()
        {
            foreach (var shape in _selectedElement)
                _mainViewModel.Shapes.Remove(shape);

            foreach (var removedLine in _removedLines)
                _mainViewModel.Lines.Remove(removedLine);
        }
    }
}