using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinarian.Application.Common;
using Veterinarian.Application.MedicalConsultations;
using Veterinarian.Application.UnitsOfWork;

namespace Veterinarian.Application.Invoices
{
    public class InvoicesServices : IInvoicesServices
    {
        private readonly InvoiceUnitOfWork _invoiceUnitOfWork;

        public InvoicesServices(InvoiceUnitOfWork invoiceUnitOfWork)
        {
            _invoiceUnitOfWork = invoiceUnitOfWork;
        }
        public async Task<Result> CreateAsync(InvoicesRequest request)
        {
            var consultation = await _invoiceUnitOfWork.MedicalConsultationRepository.GetByIdAsync(request.MedicalConsultationId);

            if(consultation is null)
            {
                return Result.Failure(MedicalConsultationError.medicalConsultationNotFound);
            }

            var invoice = new Invoice
            {
                Amount = request.Amount,
                Paid = request.Paid,
                PaymentMethod = request.PaymentMethod,
                MedicalConsultationId = request.MedicalConsultationId,
            };

            await _invoiceUnitOfWork.InvoicesRepository.AddAsync(invoice);
            await _invoiceUnitOfWork.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var invoice = await _invoiceUnitOfWork.InvoicesRepository.GetByIdAsync(id);

            if (invoice is null)
            {
                return Result.Failure(InvoicesError.InvoiceNotFound);
            }
            _invoiceUnitOfWork.InvoicesRepository.Delete(invoice);
            await _invoiceUnitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<List<InvoicesResponse>>> GetAllAsync()
        {
            var invoices = await _invoiceUnitOfWork.InvoicesRepository.GetAllAsync();

            if (invoices is null || invoices.Count() <= 0 )
            {
                return Result.Success(new List<InvoicesResponse>());
            }

            var response = invoices.Select(i => new InvoicesResponse
            {
                Amount = i.Amount,
                Paid = i.Paid,
                PaymentMethod = i.PaymentMethod,
                MedicalConsultationId = i.MedicalConsultationId,
            }).ToList();

            return Result.Success(response);

        }

        public async Task<Result<InvoicesResponse>> GetByIdAsync(Guid id)
        {
            var invoice = await _invoiceUnitOfWork.InvoicesRepository.GetByIdAsync(id);

            if(invoice is null)
            {
                return Result.Failure<InvoicesResponse>(InvoicesError.InvoiceNotFound);
            }

            var resposne = new InvoicesResponse
            {
                Amount = invoice.Amount,
                Paid = invoice.Paid,
                PaymentMethod = invoice.PaymentMethod,
                MedicalConsultationId = invoice.MedicalConsultationId,
            };

            return Result.Success(resposne);

        }

        public async Task<Result<PaginationResultDto<InvoicesResponse>>> SearchAsync(string? search, int page = 1, int pageSize = 5)
        {
            var invoinces = await _invoiceUnitOfWork.InvoicesRepository.SearchInvoices(search, page, pageSize);
            if(invoinces.Items is null || invoinces.Items.Count <= 0)
            {
                return Result.Success(new PaginationResultDto<InvoicesResponse>());
            }

            var invoicesResposne = invoinces.Items.Select(invoice => new InvoicesResponse()
            {
                Amount = invoice.Amount,
                Paid = invoice.Paid,
                PaymentMethod = invoice.PaymentMethod,
                MedicalConsultationId = invoice.MedicalConsultationId,
                MedicalConsultation = new MedicalConsulationResponse()
                {
                    AppointmentDate = invoice.MedicalConsultation.AppointmentDate,
                    AppointmentEnd = invoice.MedicalConsultation.AppointmentEnd,
                    MedicalTreatMent = invoice.MedicalConsultation.MedicalTreatMent,
                    Price = invoice.MedicalConsultation.Price,
                    PetId = invoice.MedicalConsultation.PetId,
                    Pet = new Owners.PetResources()
                    {
                        Name = invoice.MedicalConsultation.Pet!.Name,
                        BirhtDate = invoice.MedicalConsultation.Pet.BirhtDate,
                        GenderStatus = invoice.MedicalConsultation.Pet.GenderStatus,
                        Breed = invoice.MedicalConsultation.Pet.Breed,
                        Specie = invoice.MedicalConsultation.Pet.Specie,
                    },
                    Vet = new Vets.VetResponse()
                    {
                        GivenName = invoice.MedicalConsultation.Vet!.GivenName,
                        FamilyName = invoice.MedicalConsultation.Vet.FamilyName,
                        Contact = invoice.MedicalConsultation.Vet.Contact,
                        Email = invoice.MedicalConsultation.Vet.Email,
                        ProfessionalId = invoice.MedicalConsultation.Vet.ProfessionalId,
                    },
                }
            }).ToList();

            var resposne = new PaginationResultDto<InvoicesResponse>()
            {
                Items = invoicesResposne,
                Page = invoinces.Page,
                PageSize = invoinces.PageSize,
                TotalCount = invoinces.TotalCount
            };
            return Result.Success(resposne);

        }

        public async Task<Result> UpdateAsync(Guid id, InvoicesRequest resources)
        {
            var invoice = await _invoiceUnitOfWork.InvoicesRepository.GetByIdAsync(id);
            if(invoice is null)
            {
                return Result.Failure(InvoicesError.InvoiceNotFound);
            }

            var resposne = new InvoicesResponse
            {
                Amount = resources.Amount,
                Paid = resources.Paid,
                PaymentMethod = resources.PaymentMethod,
                MedicalConsultationId = resources.MedicalConsultationId,
            };

            return Result.Success();


        }
    }
}
