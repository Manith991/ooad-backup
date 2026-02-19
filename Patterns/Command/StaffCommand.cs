using System;
using OOAD_Project.Domain;
using OOAD_Project.Patterns.Repository;

namespace OOAD_Project.Patterns.Command
{
    /// <summary>
    /// COMMAND PATTERN - Add Staff/User Command
    /// </summary>
    public class AddStaffCommand : ICommand
    {
        private readonly User _user;
        private readonly IRepository<User> _repository;
        private int _insertedId = -1;

        public AddStaffCommand(User user, IRepository<User> repository)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _insertedId = _repository.Add(_user);
                _user.Id = _insertedId;
                Console.WriteLine($"[AddStaffCommand] Added staff '{_user.Name}' with ID {_insertedId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddStaffCommand] Error executing: {ex.Message}");
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
                    Console.WriteLine($"[AddStaffCommand] Undid addition of staff ID {_insertedId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AddStaffCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Add Staff: {_user.Name}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Update Staff/User Command
    /// </summary>
    public class UpdateStaffCommand : ICommand
    {
        private readonly User _newUser;
        private readonly IRepository<User> _repository;
        private User _oldUser;

        public UpdateStaffCommand(User newUser, IRepository<User> repository)
        {
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _oldUser = _repository.GetById(_newUser.Id);

                if (_oldUser == null)
                {
                    throw new InvalidOperationException($"Staff with ID {_newUser.Id} not found");
                }

                _repository.Update(_newUser);
                Console.WriteLine($"[UpdateStaffCommand] Updated staff ID {_newUser.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateStaffCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_oldUser != null)
            {
                try
                {
                    _repository.Update(_oldUser);
                    Console.WriteLine($"[UpdateStaffCommand] Restored staff ID {_oldUser.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UpdateStaffCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Update Staff: {_newUser.Name}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Delete Staff/User Command
    /// </summary>
    public class DeleteStaffCommand : ICommand
    {
        private readonly int _userId;
        private readonly IRepository<User> _repository;
        private User _deletedUser;

        public DeleteStaffCommand(int userId, IRepository<User> repository)
        {
            _userId = userId;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _deletedUser = _repository.GetById(_userId);

                if (_deletedUser == null)
                {
                    throw new InvalidOperationException($"Staff with ID {_userId} not found");
                }

                _repository.Delete(_userId);
                Console.WriteLine($"[DeleteStaffCommand] Deleted staff '{_deletedUser.Name}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteStaffCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_deletedUser != null)
            {
                try
                {
                    _repository.Add(_deletedUser);
                    Console.WriteLine($"[DeleteStaffCommand] Restored staff '{_deletedUser.Name}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DeleteStaffCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return _deletedUser != null
                ? $"Delete Staff: {_deletedUser.Name}"
                : $"Delete Staff ID: {_userId}";
        }
    }
}