using System.Windows;
using UMLaut.Model;

namespace UMLaut.UndoRedo
{
    public class MoveShapeCommand : IUndoRedoCommand
    {
        private readonly UMLShape _shape;
        private Point _oldPosition;
        private Point _newPosition;

        public MoveShapeCommand(UMLShape shape, Point oldPostion, Point newPostion)
        {
            _shape = shape;
            _oldPosition = oldPostion;
            _newPosition = newPostion;
        }

        public void Execute()
        {
            _shape.X = _newPosition.X;
            _shape.Y = _newPosition.Y;
        }

        public void UnExecute()
        {
            _shape.X = _oldPosition.X;
            _shape.Y = _oldPosition.Y;
        }
    }
}