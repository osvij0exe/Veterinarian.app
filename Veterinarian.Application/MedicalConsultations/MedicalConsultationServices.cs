using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Vets;
using Veterinarian.Application.Common;
using Veterinarian.Application.Owners;
using Veterinarian.Application.Specialities;
using Veterinarian.Application.UnitsOfWork;

namespace Veterinarian.Application.MedicalConsultations
{
    public class MedicalConsultationServices : IMedicalConsultationServices
    {
        private readonly MedicalConsultationUnitOfWork _medicalConsultationUnitOfWork;

        public MedicalConsultationServices(MedicalConsultationUnitOfWork medicalConsultationUnitOfWork)
        {
            _medicalConsultationUnitOfWork = medicalConsultationUnitOfWork;
        }
        public async Task<Result> CreateAsync(MedicalConsultationRequest request)
        {

            var pet = await _medicalConsultationUnitOfWork.PetsRepository.GetByIdAsync(request.PetId);
            var vet= await _medicalConsultationUnitOfWork.VetsRepository.GetByIdAsync(request.VetId);

            if (pet is null)
            {
                return Result.Failure(PetError.PetNotFound);
            };
            if (vet is null)
            {
                return Result.Failure(VetsError.VetNotFoud);
            };



            var consultation = new MedicalConsultation()
            {
                AppointmentDate = request.AppointmentDate,
                AppointmentEnd = request.AppointmentDate.AddMinutes(request.Duration),
                MedicalTreatMent = request.MedicalTreatMent,
                Price = request.Price,
                PetId = pet.Id,
                VetId = vet.Id,

            };
            await _medicalConsultationUnitOfWork.MedicalConsultationRepository.AddAsync(consultation);
            await _medicalConsultationUnitOfWork.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var consultation = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.GetByIdAsync(id);

            if(consultation is null)
            {
                return Result.Failure(MedicalConsultationError.medicalConsultationNotFound);
            }

            _medicalConsultationUnitOfWork.MedicalConsultationRepository.Delete(consultation);
            await _medicalConsultationUnitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<List<MedicalConsulationResponse>>> GetAllAsync()
        {

            var relationShips = "Vet,Pet";

            var consultations = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.GetAllAsync(relationShips);

            if(consultations is null || consultations.Count() <= 0)
            {
                return Result.Success(new List<MedicalConsulationResponse>());
            }

            

            var response = consultations.Select(c => new MedicalConsulationResponse
            {
                AppointmentDate = c.AppointmentDate,
                AppointmentEnd = c.AppointmentEnd,
                MedicalTreatMent = c.MedicalTreatMent,
                Price = c.Price,
                PetId = c.PetId,
                Pet = new PetResources
                {
                    Name = c.Pet!.Name,
                    BirhtDate = c.Pet.BirhtDate,
                    Breed = c.Pet.Breed,
                    GenderStatus = c.Pet.GenderStatus,
                    Specie =c.Pet.Specie
                    
                },
                Vet = new Vets.VetResponse
                {
                    FamilyName = c.Vet!.FamilyName,
                    GivenName = c.Vet.GivenName,
                    Contact = c.Vet.Contact,
                    Email = c.Vet.Email,
                    ProfessionalId = c.Vet.ProfessionalId,
                    SpecialityId = c.Vet.SpecialityId

                }
            }).ToList();

            return Result.Success(response);

        }

        public async Task<Result<MedicalConsulationResponse>> GetByIdAsync(Guid id)
        {
            var relationShips = "Vet,Pet";

            var consultation = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.GetByIdAsync(id,relationships: relationShips);

            if(consultation is null)
            {
                return Result.Failure<MedicalConsulationResponse>(MedicalConsultationError.medicalConsultationNotFound);
            }

            var response = new MedicalConsulationResponse
            {
                AppointmentDate = consultation.AppointmentDate,
                AppointmentEnd = consultation.AppointmentEnd,
                MedicalTreatMent = consultation.MedicalTreatMent,
                Price = consultation.Price,
                PetId = consultation.PetId,
                Pet = new PetResources
                {
                    Name = consultation.Pet!.Name,
                    Specie = consultation.Pet.Specie,
                    Breed = consultation.Pet.Breed,
                    BirhtDate = consultation.Pet.BirhtDate,
                    GenderStatus = consultation.Pet.GenderStatus,
                },
                Vet = new Vets.VetResponse
                {
                    FamilyName = consultation.Vet!.FamilyName,
                    GivenName = consultation.Vet.GivenName,
                    Contact = consultation.Vet.Contact,
                    Email = consultation.Vet.Email,
                    ProfessionalId = consultation.Vet.ProfessionalId,
                    SpecialityId = consultation.Vet.SpecialityId
                }
            };

            return Result.Success(response);


        }

        public async Task<Result<List<MedicalConsulationResponse>>> SearchConsultationByBetOrVetAsync(string? search)
        {
            

            var consultations = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.SearchConsultationByPetOrVetAsync(search);

            if (consultations is null || consultations.Count <= 0)
            {
                return Result.Success(new List<MedicalConsulationResponse>());
            }



            var response = consultations.Select(c => new MedicalConsulationResponse
            {
                AppointmentDate = c.AppointmentDate,
                AppointmentEnd = c.AppointmentEnd,
                MedicalTreatMent = c.MedicalTreatMent,
                Price = c.Price,
                PetId = c.PetId,
                Pet = new PetResources
                {
                    Name = c.Pet!.Name,
                    BirhtDate = c.Pet.BirhtDate,
                    Breed = c.Pet.Breed,
                    GenderStatus = c.Pet.GenderStatus,
                    Specie = c.Pet.Specie

                },
                Vet = new Vets.VetResponse
                {
                    FamilyName = c.Vet!.FamilyName,
                    GivenName = c.Vet.GivenName,
                    Contact = c.Vet.Contact,
                    Email = c.Vet.Email,
                    ProfessionalId = c.Vet.ProfessionalId,
                    SpecialityId = c.Vet.SpecialityId,
                    Speciality = new SpecialitiesResponse
                    {
                        Name = c.Vet.Speciality.Name
                    }
                }
            }).ToList();

            return Result.Success(response);
        }

        public async Task<Result<PaginationResultDto<MedicalConsulationResponse>>> SearchConsultationByBetOrVetAsync(string? search, int page, int pageSize)
        {
            var consultations = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.SearchConsultationByPetOrVetAsync(search,page,pageSize);

            if (consultations.Items is null || consultations.Items.Count <= 0)
            {
                return Result.Success(new PaginationResultDto<MedicalConsulationResponse>());
            }



            var consulationResponse = consultations.Items.Select(c => new MedicalConsulationResponse
            {
                AppointmentDate = c.AppointmentDate,
                AppointmentEnd = c.AppointmentEnd,
                MedicalTreatMent = c.MedicalTreatMent,
                Price = c.Price,
                PetId = c.PetId,
                Pet = new PetResources
                {
                    Name = c.Pet!.Name,
                    BirhtDate = c.Pet.BirhtDate,
                    Breed = c.Pet.Breed,
                    GenderStatus = c.Pet.GenderStatus,
                    Specie = c.Pet.Specie

                },
                Vet = new Vets.VetResponse
                {
                    FamilyName = c.Vet!.FamilyName,
                    GivenName = c.Vet.GivenName,
                    Contact = c.Vet.Contact,
                    Email = c.Vet.Email,
                    ProfessionalId = c.Vet.ProfessionalId,
                    SpecialityId = c.Vet.SpecialityId,
                    Speciality = new SpecialitiesResponse
                    {
                        Name = c.Vet.Speciality.Name
                    }
                }
            }).ToList();

            var response = new PaginationResultDto<MedicalConsulationResponse>()
            {
                Items = consulationResponse,
                Page = consultations.Page,
                PageSize = consultations.PageSize,
                TotalCount = consultations.TotalCount,
            };
            return Result.Success(response);

        }

        public async Task<Result> UpdateAsync(Guid id, MedicalConsultationRequest resources)
        {
            var consultation = await _medicalConsultationUnitOfWork.MedicalConsultationRepository.GetByIdAsync(id);

            if(consultation is null)
            {
                return Result.Failure(MedicalConsultationError.medicalConsultationNotFound);
            }

            consultation.AppointmentDate = resources.AppointmentDate;
            consultation.AppointmentEnd = resources.AppointmentDate.AddMinutes(resources.Duration);
            consultation.MedicalTreatMent = resources.MedicalTreatMent;
            consultation.Price = resources.Price;
            consultation.PetId = resources.PetId;

            _medicalConsultationUnitOfWork.MedicalConsultationRepository.UpdateAsync(consultation);
            await _medicalConsultationUnitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
