using System;
using OOAD_Project.Domain;
using OOAD_Project.Patterns.Repository;

namespace OOAD_Project.Patterns.Command
{
    /// <summary>
    /// COMMAND PATTERN - Add Table Command
    /// </summary>
    public class AddTableCommand : ICommand
    {
        private readonly Table _table;
        private readonly IRepository<Table> _repository;
        private int _insertedId = -1;

        public AddTableCommand(Table table, IRepository<Table> repository)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _insertedId = _repository.Add(_table);
                _table.TableId = _insertedId;
                Console.WriteLine($"[AddTableCommand] Added table '{_table.TableName}' with ID {_insertedId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddTableCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_insertedId > 0)
            {
                try
                {
                    _repository.Delete(_insertedId);
                    Console.WriteLine($"[AddTableCommand] Undid addition of table ID {_insertedId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AddTableCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Add Table: {_table.TableName}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Update Table Command
    /// </summary>
    public class UpdateTableCommand : ICommand
    {
        private readonly Table _newTable;
        private readonly IRepository<Table> _repository;
        private Table _oldTable;

        public UpdateTableCommand(Table newTable, IRepository<Table> repository)
        {
            _newTable = newTable ?? throw new ArgumentNullException(nameof(newTable));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _oldTable = _repository.GetById(_newTable.TableId);

                if (_oldTable == null)
                {
                    throw new InvalidOperationException($"Table with ID {_newTable.TableId} not found");
                }

                _repository.Update(_newTable);
                Console.WriteLine($"[UpdateTableCommand] Updated table ID {_newTable.TableId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateTableCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_oldTable != null)
            {
                try
                {
                    _repository.Update(_oldTable);
                    Console.WriteLine($"[UpdateTableCommand] Restored table ID {_oldTable.TableId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UpdateTableCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Update Table: {_newTable.TableName}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Delete Table Command
    /// </summary>
    public class DeleteTableCommand : ICommand
    {
        private readonly int _tableId;
        private readonly IRepository<Table> _repository;
        private Table _deletedTable;

        public DeleteTableCommand(int tableId, IRepository<Table> repository)
        {
            _tableId = tableId;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _deletedTable = _repository.GetById(_tableId);

                if (_deletedTable == null)
                {
                    throw new InvalidOperationException($"Table with ID {_tableId} not found");
                }

                _repository.Delete(_tableId);
                Console.WriteLine($"[DeleteTableCommand] Deleted table '{_deletedTable.TableName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteTableCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_deletedTable != null)
            {
                try
                {
                    _repository.Add(_deletedTable);
                    Console.WriteLine($"[DeleteTableCommand] Restored table '{_deletedTable.TableName}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DeleteTableCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return _deletedTable != null
                ? $"Delete Table: {_deletedTable.TableName}"
                : $"Delete Table ID: {_tableId}";
        }
    }
}