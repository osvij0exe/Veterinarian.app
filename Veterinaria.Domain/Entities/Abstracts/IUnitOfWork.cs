using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Abstracts
{
    public interface IUnitOfWork
    {
        void RollBasck();
        Task SaveChangesAsync();
        Task RollBasckTransacctionAsync();
    }
}
