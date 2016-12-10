using System.Windows;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class MoveShapeCommand : IUndoRedoCommand
    {
        private readonly ShapeViewModel _movedShape;
        private Point _oldPosition;
        private Point _newPosition;

        public MoveShapeCommand(ShapeViewModel movedShape, Point oldPosition, Point newPosition)
        {
            _movedShape = movedShape;
            _oldPosition = oldPosition;
            _newPosition = newPosition;
        }

        public void Undo()
        {
            _movedShape.X = _oldPosition.X;
            _movedShape.Y = _oldPosition.Y;
        }

        public void Redo()
        {
            _movedShape.X = _newPosition.X;
            _movedShape.Y = _newPosition.Y;
        }
    }
}