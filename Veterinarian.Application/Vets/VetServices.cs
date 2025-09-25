using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Sécialities;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;
using Veterinarian.Application.Common;
using Veterinarian.Application.UnitsOfWork;


namespace Veterinarian.Application.Vets
{
    public class VetServices : IVetServices
    {
        private readonly VetsUnitOfWork _vetsUnitOfWork;

        public VetServices(VetsUnitOfWork vetsUnitOfWork)
        {
            _vetsUnitOfWork = vetsUnitOfWork;
        }

        public async Task<Result> CreateAndRegisterAsync(VetRequest request, IdentityUser identityUser)
        {
            //transacción entre tablas dentro de la misma base de datos
            using IDbContextTransaction transaction = await _vetsUnitOfWork._applicationIdentityDbContext.Database.BeginTransactionAsync();
            _vetsUnitOfWork._applicationDbContext.Database.SetDbConnection(_vetsUnitOfWork._applicationIdentityDbContext.Database.GetDbConnection());
            await _vetsUnitOfWork._applicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());



            //Aplication User ApplicationDbContext
            User user = new User
            {
                Id = Guid.CreateVersion7().ToString(),
                Name = request.Email,
                Email = request.Email,
                CreateAtUtc = DateTime.UtcNow,
            };
            user.IdentityId = identityUser.Id;

            await _vetsUnitOfWork.UserRepository.AddAsync(user);
            //await _vetsUnitOfWork.SaveChangesAsync();


            var speciality = await _vetsUnitOfWork.SpecialityRepository.GetByIdAsync(request.SpecialityId);

            if(speciality is null)
            {
                return Result.Failure(SpecialityError.SpecialityNotFound);
            }

            var vet = new Vet()
            {
                UserId = user.Id,
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                ProfessionalId = request.ProfessionalId,
                Email = request.Email,
                Contact = request.Contact,
                SpecialityId = speciality.Id,

            };

            await _vetsUnitOfWork.VetsRepository.AddAsync(vet);
            await _vetsUnitOfWork.SaveChangesAsync();

            await _vetsUnitOfWork._applicationIdentityDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            //transacción entre tablas dentro de la misma base de datos
            using IDbContextTransaction transaction = await _vetsUnitOfWork._applicationIdentityDbContext.Database.BeginTransactionAsync();
            _vetsUnitOfWork._applicationDbContext.Database.SetDbConnection(_vetsUnitOfWork._applicationIdentityDbContext.Database.GetDbConnection());
            await _vetsUnitOfWork._applicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var vet = await _vetsUnitOfWork.VetsRepository.GetByIdAsync(id);
                      
            if (vet is null)
            {
                return Result.Failure(VetsError.VetNotFoud);
            }

            var user = await _vetsUnitOfWork.UserRepository.GetUserByIdAsync(vet.UserId);

            IdentityUser identityUser =  await _vetsUnitOfWork.IAplicaionUserRepository.GetUserById(user.IdentityId);
            
            if(identityUser is null  || user is null)
            {
                return Result.Failure(VetsError.VetNotFoud);
            }

            _vetsUnitOfWork.VetsRepository.Delete(vet);
            await _vetsUnitOfWork.IAplicaionUserRepository.DelectUserAsync(identityUser);
            await _vetsUnitOfWork.SaveChangesAsync();
            await _vetsUnitOfWork._applicationIdentityDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();

        }

        public async Task<Result<List<VetResponse>>> GetAllAsync()
        {

            var relationShips = "Speciality";


            var vets = await _vetsUnitOfWork.VetsRepository.GetAllAsync(relatiohShips:relationShips);

            var vetsResponse = vets.Select(vet => new VetResponse
            {
                GivenName = vet.GivenName,
                FamilyName = vet.FamilyName,
                ProfessionalId = vet.ProfessionalId,
                Email = vet.Email,
                Contact = vet.Contact,
                SpecialityId = vet.SpecialityId,
                Speciality = new Specialities.SpecialitiesResponse
                {
                    Name = vet.Speciality.Name,
                }
            }).ToList();

            if(vetsResponse is null || vetsResponse.Count() <= 0  )
            {
                return Result.Success(new List<VetResponse>());
            }


            return Result.Success(vetsResponse);


        }

        public async Task<Result<VetResponse>> GetByIdAsync(Guid id)
        {

            var relationShips = "Speciality";

            var vet = await _vetsUnitOfWork.VetsRepository.GetByIdAsync(id,relationships: relationShips);

            if(vet is null)
            {
                return Result.Failure<VetResponse>(VetsError.VetNotFoud);
            }



            var vetResponse = new VetResponse
            {
                GivenName = vet.GivenName,
                FamilyName = vet.FamilyName,
                ProfessionalId = vet.ProfessionalId,
                Email = vet.Email,
                Contact = vet.Contact,
                SpecialityId = vet.SpecialityId,
                Speciality = new Specialities.SpecialitiesResponse()
                {
                    Name = vet.Speciality.Name,
                }
            };
            return Result.Success(vetResponse);

        }

        public async Task<Result<List<VetResponse>>> SearchVetAsync(string? search)
        {
            var vets = await _vetsUnitOfWork.VetsRepository.SearchVet(search);

            var vetsResponse = vets.Select(vet => new VetResponse
            {
                GivenName = vet.GivenName,
                FamilyName = vet.FamilyName,
                ProfessionalId = vet.ProfessionalId,
                Email = vet.Email,
                Contact = vet.Contact,
                SpecialityId = vet.SpecialityId,
                Speciality = new Specialities.SpecialitiesResponse
                {
                    Name = vet.Speciality.Name,
                }
            }).ToList();

            if (vetsResponse is null || vetsResponse.Count() <= 0)
            {
                return Result.Success(new List<VetResponse>());
            }


            return Result.Success(vetsResponse);
        }

        public async Task<Result<PaginationResultDto<VetResponse>>> SearchVetAsync(string? search, int page = 1, int pageSize = 5)
        {
            var vets = await _vetsUnitOfWork.VetsRepository.SearchVet(search,page,pageSize);

            if (vets.Items is null || vets.Items.Count == 0)
            {
                return Result.Success(new PaginationResultDto<VetResponse>());
            }

            var vetsResponse = vets.Items.Select(vet => new VetResponse
            {
                GivenName = vet.GivenName,
                FamilyName = vet.FamilyName,
                ProfessionalId = vet.ProfessionalId,
                Email = vet.Email,
                Contact = vet.Contact,
                SpecialityId = vet.SpecialityId,
                Speciality = new Specialities.SpecialitiesResponse
                {
                    Name = vet.Speciality.Name,
                }
            }).ToList();

            PaginationResultDto<VetResponse> response = new PaginationResultDto<VetResponse>()
            {
                Items = vetsResponse,
                Page = vets.Page,
                PageSize = vets.PageSize,
                TotalCount = vets.TotalCount
            };


            return Result.Success(response);

        }

        public async Task<Result> UpdateAsync(Guid id, VetRequest resources, string userId)
        {
            var vet = await _vetsUnitOfWork.VetsRepository.GetByIdAsync(id);

            if(vet is null && vet!.UserId != userId)
            {
                return Result.Failure(VetsError.VetNotFoud);
            }



            var speciality = await _vetsUnitOfWork.SpecialityRepository.GetByIdAsync(resources.SpecialityId);

            if (speciality is null)
            {
                return Result.Failure(SpecialityError.SpecialityNotFound);
            }


            vet.GivenName = resources.GivenName;
            vet.FamilyName = resources.FamilyName;
            vet.ProfessionalId = resources.ProfessionalId;
            vet.Email = resources.Email;
            vet.Contact = resources.Contact;
            vet.SpecialityId = resources.SpecialityId;

            _vetsUnitOfWork.VetsRepository.UpdateAsync(vet);
            await _vetsUnitOfWork.SaveChangesAsync();
            return Result.Success();

        }
    }
}
