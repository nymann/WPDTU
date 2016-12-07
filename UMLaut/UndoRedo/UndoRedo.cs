using System;
using System.Collections.Generic;

namespace UMLaut.UndoRedo
{
    public class UndoRedo
    {
        private readonly Stack<IUndoRedoCommand> _undoCommands = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> _redoCommands = new Stack<IUndoRedoCommand>();

        public EventHandler EnableUndoRedo;

        /// <summary>
        /// Levels refers to how many times you want to redo.
        /// </summary>
        /// <param name="levels"></param>
        public void Redo(int levels)
        {
            for (var i = 1; i <= levels; i++)
            {
                if (_redoCommands.Count == 0) continue;
                var command = _redoCommands.Pop();
                command.UnExecute();
                _undoCommands.Push(command);
            }
            // If the UndoRedo feature is enabled, disable it.
            EnableUndoRedo?.Invoke(null, null);
        }

        /// <summary>
        /// Levels refers to how many times you want to Undo.
        /// </summary>
        /// <param name="levels"></param>
        public void Undo(int levels)
        {
            for (var i = 1; i <= levels; i++)
            {
                if (_undoCommands.Count == 0) continue;
                var command = _undoCommands.Pop();
                command.Execute();
                _redoCommands.Push(command);
            }
            // If the UndoRedo feature is enabled, disable it.
            EnableUndoRedo?.Invoke(null, null);
        }

        public void InsertInUndoRedo(IUndoRedoCommand command)
        {
            _undoCommands.Push(command);
            _redoCommands.Clear();
        }
    }
}