using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Repositories;

namespace App.Infrastructure.Repositories.Implementation;

/// <summary>
/// Implementation of Unit of Work supporting both generic and potential explicit patterns.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _Context;
    private readonly Dictionary<Type, object> _Repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _Context = context;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (!_Repositories.ContainsKey(type))
        {
            _Repositories[type] = new Repository<T>(_Context);
        }
        return (IRepository<T>)_Repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _Context.Dispose();
    }
}
