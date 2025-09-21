using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Application.UnitsOfWork
{
    public class InvoiceUnitOfWork : UnitOfWork
    {
        public InvoiceUnitOfWork(ApplicationDbContext dbContext,
            IInvoicesRepository invoicesRepository,
            IMedicalConsultationRepository medicalConsultationRepository,
            IUserContext userContext) 
            : base(dbContext)
        {
            MedicalConsultationRepository = medicalConsultationRepository;
            InvoicesRepository = invoicesRepository;
            UserContext = userContext;
        }

        public IUserContext UserContext;
        public IMedicalConsultationRepository MedicalConsultationRepository { get; }
        public IInvoicesRepository InvoicesRepository { get; }
    }
}
