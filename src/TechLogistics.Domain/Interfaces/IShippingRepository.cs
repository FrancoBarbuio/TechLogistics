using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLogistics.Domain.Entities;

namespace TechLogistics.Domain.Interfaces
{
    public interface IShippingRepository
    {
        // Usamos Task para asincronía (Vital en .NET moderno)
        Task<Shipping> GetByIdAsync(int id);
        Task<IEnumerable<Shipping>> GetAllAsync();
        Task AddAsync(Shipping shipping);
        Task UpdateAsync(Shipping shipping);

        // No necesitamos Delete para este ejemplo, pero lo podrías agregar
    }
}