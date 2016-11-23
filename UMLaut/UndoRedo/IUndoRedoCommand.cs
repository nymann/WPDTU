namespace UMLaut.UndoRedo
{
    public interface IUndoRedoCommand
    {
        void Execute();
        void UnExecute();
    }
}