using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLogistics.Domain.Entities;
using TechLogistics.Domain.Interfaces;

namespace TechLogistics.Infrastructure.Persistence
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly AppDbContext _context;

        public ShippingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Shipping shipping)
        {
            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await _context.Shippings.ToListAsync();
        }

        public async Task<Shipping> GetByIdAsync(int id)
        {
            return await _context.Shippings.FindAsync(id);
        }

        public async Task UpdateAsync(Shipping shipping)
        {
            _context.Shippings.Update(shipping);
            await _context.SaveChangesAsync();
        }
    }
}
