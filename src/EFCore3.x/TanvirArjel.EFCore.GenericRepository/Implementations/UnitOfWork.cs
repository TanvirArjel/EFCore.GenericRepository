// <copyright file="UnitOfWork.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository.Services;

namespace TanvirArjel.EFCore.GenericRepository.Implementations
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        private Hashtable _repositories;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWorkRepository<T> Repository<T>()
            where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            string type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                Type repositoryType = typeof(UnitOfWorkRepository<>);

                object repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IUnitOfWorkRepository<T>)_repositories[type];
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public void ResetContextState()
        {
            _dbContext.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                .ForEach(e => e.State = EntityState.Detached);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
