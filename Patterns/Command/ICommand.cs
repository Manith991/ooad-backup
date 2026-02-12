using System;

namespace OOAD_Project.Patterns.Command
{
    /// <summary>
    /// COMMAND PATTERN - Base command interface
    /// Encapsulates operations as objects with undo/redo support
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
        string GetDescription();
    }

    /// <summary>
    /// Command result to track success/failure
    /// </summary>
    public class CommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public CommandResult(bool success, string message = "", object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}