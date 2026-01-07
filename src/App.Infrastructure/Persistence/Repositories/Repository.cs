using App.Domain.Entities;
using App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository implementation using EF Core.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _Context;
    protected readonly DbSet<T> _DbSet;

    public Repository(AppDbContext context)
    {
        _Context = context;
        _DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _DbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _DbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(T entity)
    {
        _DbSet.Remove(entity);
        await Task.CompletedTask;
    }
}
