using System;
using System.Collections.Generic;
using System.Linq;

namespace OOAD_Project.Patterns.Command
{
    /// <summary>
    /// COMMAND PATTERN - Command Invoker
    /// Manages command execution, undo, and redo operations
    /// </summary>
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        private readonly int _maxHistorySize;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;
        public int UndoCount => _undoStack.Count;
        public int RedoCount => _redoStack.Count;

        public CommandInvoker(int maxHistorySize = 50)
        {
            _maxHistorySize = maxHistorySize;
        }

        /// <summary>
        /// Execute a command and add it to the undo stack
        /// </summary>
        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                command.Execute();

                // Clear redo stack when new command is executed
                _redoStack.Clear();

                // Add to undo stack
                _undoStack.Push(command);

                // Maintain max history size
                if (_undoStack.Count > _maxHistorySize)
                {
                    var items = _undoStack.ToList();
                    items.RemoveAt(items.Count - 1); // Remove oldest
                    _undoStack.Clear();
                    foreach (var item in items.AsEnumerable().Reverse())
                    {
                        _undoStack.Push(item);
                    }
                }

                Console.WriteLine($"[CommandInvoker] Executed: {command.GetDescription()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CommandInvoker] Failed to execute command: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Undo the last executed command
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                Console.WriteLine("[CommandInvoker] Nothing to undo");
                return;
            }

            var command = _undoStack.Pop();

            try
            {
                command.Undo();
                _redoStack.Push(command);
                Console.WriteLine($"[CommandInvoker] Undid: {command.GetDescription()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CommandInvoker] Failed to undo command: {ex.Message}");
                // Put command back on undo stack if undo fails
                _undoStack.Push(command);
                throw;
            }
        }

        /// <summary>
        /// Redo the last undone command
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                Console.WriteLine("[CommandInvoker] Nothing to redo");
                return;
            }

            var command = _redoStack.Pop();

            try
            {
                command.Execute();
                _undoStack.Push(command);
                Console.WriteLine($"[CommandInvoker] Redid: {command.GetDescription()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CommandInvoker] Failed to redo command: {ex.Message}");
                // Put command back on redo stack if redo fails
                _redoStack.Push(command);
                throw;
            }
        }

        /// <summary>
        /// Clear all command history
        /// </summary>
        public void ClearHistory()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            Console.WriteLine("[CommandInvoker] Command history cleared");
        }

        /// <summary>
        /// Get the description of the command that would be undone
        /// </summary>
        public string GetUndoDescription()
        {
            return CanUndo ? _undoStack.Peek().GetDescription() : "Nothing to undo";
        }

        /// <summary>
        /// Get the description of the command that would be redone
        /// </summary>
        public string GetRedoDescription()
        {
            return CanRedo ? _redoStack.Peek().GetDescription() : "Nothing to redo";
        }

        /// <summary>
        /// Get all undo history descriptions
        /// </summary>
        public IEnumerable<string> GetUndoHistory()
        {
            return _undoStack.Select(cmd => cmd.GetDescription());
        }

        /// <summary>
        /// Get all redo history descriptions
        /// </summary>
        public IEnumerable<string> GetRedoHistory()
        {
            return _redoStack.Select(cmd => cmd.GetDescription());
        }
    }
}