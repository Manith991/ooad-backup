using System.Collections.Generic;

namespace OOAD_Project.Patterns.Repository
{
    /// <summary>
    /// REPOSITORY PATTERN - Generic repository interface
    /// Provides a common interface for data access operations
    /// </summary>
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        void Update(T entity);
        void Delete(int id);
        bool Exists(int id);
    }
}