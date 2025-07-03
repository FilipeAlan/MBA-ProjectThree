﻿using BuildingBlocks.Common;

namespace PagamentoContext.Infrastructure.Context
{
    public class UnitOfWork : IPagamentoUnityOfWork
    {
        private readonly PagamentoDbContext _context;

        public UnitOfWork(PagamentoDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
