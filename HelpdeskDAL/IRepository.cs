// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This template defines a generic repository, providing CRUD operations for entities of any type.
// ============================================================================

using System.Linq.Expressions;
namespace HelpdeskDAL
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetSome(Expression<Func<T, bool>> match);
        Task<T?> GetOne(Expression<Func<T, bool>> match);
        Task<T> Add(T entity);
        Task<UpdateStatus> Update(T enity);
        Task<int> Delete(int i);
    }
}