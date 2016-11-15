using System;
using System.Windows;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        //private Point _oldPosition;

        public DeleteCommand(/*Point oldPosition*/)
        {
            //oldPosition = _oldPosition;
        }

        public void Execute()
        {
            Console.WriteLine(@"Undo requested.");
        }

        public void UnExecute()
        {
            Console.WriteLine(@"Redo requested.");            
        }
    }
}