
using System.Windows;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class MoveShapeCommand : IUndoRedoCommand
    {
        private ShapeViewModel _movedShape;
        private Point _oldPosition;
        private Point _newPosition;

        public MoveShapeCommand(ShapeViewModel movedShape, Point oldPosition, Point newPosition)
        {
            _movedShape = movedShape;
            _oldPosition = oldPosition;
            _newPosition = newPosition;
        }

        public void Execute()
        {
            // undo
            _movedShape.X = _oldPosition.X;
            _movedShape.Y = _oldPosition.Y;
        }

        public void UnExecute()
        {
            _movedShape.X = _newPosition.X;
            _movedShape.Y = _newPosition.Y;
        }
    }
}