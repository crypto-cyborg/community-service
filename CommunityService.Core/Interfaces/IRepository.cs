﻿using System.Linq.Expressions;

namespace CommunityService.Core.Interfaces;

public interface IRepository<TEntity>
{
    IQueryable<TEntity> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<TEntity?> GetByIdAsync(object id, string includeProperties = "");
    Task InsertAsync(TEntity entity);
    void Delete(object id);
    void Delete(TEntity entityToDelete);
    void Update(TEntity entityToUpdate);
}
