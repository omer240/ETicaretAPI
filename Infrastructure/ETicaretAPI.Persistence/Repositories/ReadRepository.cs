﻿using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext _context;
        public ReadRepository(ETicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table 
            => _context.Set<T>();


        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable(); // Table: DbSet<T> zaten IQueryable<T>
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }


        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if(!tracking)
                query = query.AsNoTracking();
            return query;

        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if(!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            //=> await Table.FirstOrDefaultAsync(data => data.Id==Guid.Parse(id));

            var guid = Guid.Parse(id);
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(e => e.Id == guid);
            //return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == guid);
        }

    }
}
