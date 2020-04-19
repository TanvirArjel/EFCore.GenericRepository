// <copyright file="IUnitOfWork.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using EFCore.GenericRepository.Repository;

namespace EFCore.GenericRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>()
            where T : class;

        int ExecuteSqlCommand(string sql, params object[] parameters);

        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        void ResetContextState();

        Task SaveChangesAsync();
    }
}
