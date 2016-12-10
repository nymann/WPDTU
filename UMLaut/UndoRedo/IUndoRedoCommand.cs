namespace UMLaut.UndoRedo
{
    public interface IUndoRedoCommand
    {
        void Undo();
        void Redo();
    }
}