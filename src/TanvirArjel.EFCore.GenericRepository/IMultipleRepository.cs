// <copyright file="IRepository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    /// <typeparam name="TContext">For Multiple Context Support</typeparam>
    public interface IMultipleRepository<TContext>: IBaseRepository
    {
     
    }
}
