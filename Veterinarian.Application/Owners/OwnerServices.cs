using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.Common;
using Veterinarian.Application.Pets;
using Veterinarian.Application.UnitsOfWork;

namespace Veterinarian.Application.Owners
{
    public class OwnerServices : IOwnerServices
    {
        private readonly OwnerUnitOfWork _ownerUnitOfWork;

        public OwnerServices(OwnerUnitOfWork ownerUnitOfWork)
        {
            _ownerUnitOfWork = ownerUnitOfWork;
        }
        public async Task<Result> CreateAndRegisterAsync(OwnerRequest request,IdentityUser identityUser)
        {
            //transaction between tables
            using IDbContextTransaction transaction = await _ownerUnitOfWork._identityDbContext.Database.BeginTransactionAsync();
            _ownerUnitOfWork._applicationDbContext.Database.SetDbConnection(_ownerUnitOfWork._identityDbContext.Database.GetDbConnection());
            await _ownerUnitOfWork._applicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());
            
            //Create Aplication user
            User user = new User
            {
                Id = Guid.CreateVersion7().ToString(),
                Name = request.Email,
                Email = request.Email,
                CreateAtUtc = DateTime.UtcNow,
            };
            user.IdentityId = identityUser.Id;

            await _ownerUnitOfWork.UserRepository.AddAsync(user);


            var pets = new List<Pet>();
            pets.AddRange(request.Pets!.Select(p => new Pet
            {
                Name = p.Name,
                Specie = p.Specie,
                Breed = p.Breed,
                UserId = user.Id,
                GenderStatus = p.GenderStatus,
                BirhtDate = p.BirhtDate
            }));

           
            var owner = new Owner
            {
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                Contact = request.Contact,
                Email = request.Email,
                UserId = user.Id,
                Pets = pets
            };
            await _ownerUnitOfWork.OwnerRepositoy.AddAsync(owner);
            await _ownerUnitOfWork.SaveChangesAsync();

            await _ownerUnitOfWork._identityDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();

        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var owner = await _ownerUnitOfWork.OwnerRepositoy.GetByIdAsync(id);
            if(owner is null)
            {
                return Result.Failure(OwnerError.OwnerNotFound);
            }

            _ownerUnitOfWork.OwnerRepositoy.Delete(owner);
            await _ownerUnitOfWork.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result<List<OwnerResponse>>> GetAllAsync()
        {
            
            var owners = await _ownerUnitOfWork.OwnerRepositoy.GetAllAsync();

            if(owners is null || owners.Count() <= 0)
            {
                return Result.Success(new List<OwnerResponse>());
            }

            var response = owners.Select(o => new OwnerResponse
            {
                OwnerId = o.Id,
                GivenName = o.GivenName,
                FamilyName = o.FamilyName,
                Contact = o.Contact,
                Email = o.Email,
            }).ToList();


            return Result.Success(response);

        }

        public async Task<Result<OwnerResponse>> GetByIdAsync(Guid id)
        {
            
            var onwer = await _ownerUnitOfWork.OwnerRepositoy.GetByIdAsync(id);

            if(onwer is null)
            {
                return Result.Failure<OwnerResponse>(OwnerError.OwnerNotFound);
            }


            var response = new OwnerResponse
            {
                OwnerId = onwer.Id,
                GivenName = onwer.GivenName,
                FamilyName = onwer.FamilyName,
                Contact = onwer.Contact,
                Email = onwer.Email,
            };
            return Result.Success(response);

        }

        public async Task<Result<List<OwnerResponse>>> SearchOnwers(string? search)
        {
            var owners = await _ownerUnitOfWork.OwnerRepositoy.SearchOwners(search);

            if (owners is null || owners.Count <= 0)
            {
                return Result.Success(new List<OwnerResponse>());
            }

            var response = owners.Select(o => new OwnerResponse
            {
                OwnerId = o.Id,
                GivenName = o.GivenName,
                FamilyName = o.FamilyName,
                Contact = o.Contact,
                Email = o.Email,
                Pets = o.Pets.Select(o => new PetResponse
                {
                    Name =o.Name,
                    Specie = o.Specie,
                    Breed = o.Breed,
                    BirhtDate = o.BirhtDate,   
                    GenderStatus = o.GenderStatus,
                }).ToList()
            }).ToList();


            return Result.Success(response);
        }

        public async Task<Result<PaginationResultDto<OwnerResponse>>> SearchOnwers(string? search, int page = 1, int pageSize = 5)
        {
            var owners = await _ownerUnitOfWork.OwnerRepositoy.SearchOwners(search,page,pageSize);

            if (owners.Items is null || owners.Items.Count <= 0)
            {
                return Result.Success(new PaginationResultDto<OwnerResponse>());
            }

            var owneresResposne = owners.Items.Select(o => new OwnerResponse
            {
                OwnerId = o.Id,
                GivenName = o.GivenName,
                FamilyName = o.FamilyName,
                Contact = o.Contact,
                Email = o.Email,
                Pets = o.Pets.Select(o => new PetResponse
                {
                    Name = o.Name,
                    Specie = o.Specie,
                    Breed = o.Breed,
                    BirhtDate = o.BirhtDate,
                    GenderStatus = o.GenderStatus,
                }).ToList()
            }).ToList();

            var response = new PaginationResultDto<OwnerResponse>()
            {
                Items = owneresResposne,
                Page = owners.Page,
                PageSize = owners.PageSize,
                TotalCount = owners.TotalCount
            };
            return Result.Success(response);

        }

        public async Task<Result> UpdateAsync(Guid id, OwnerUpdateRequest resources)
        {
            var owner = await _ownerUnitOfWork.OwnerRepositoy.GetByIdAsync(id);

            if(owner is null)
            {
                return Result.Failure(OwnerError.OwnerNotFound);
            }

            owner.GivenName = resources.GivenName;
            owner.FamilyName = resources.FamilyName;
            owner.Contact = resources.Contact;
            owner.Email = resources.Email;

            _ownerUnitOfWork.OwnerRepositoy.UpdateAsync(owner);
            await _ownerUnitOfWork.SaveChangesAsync();  
            return Result.Success();

        }
    }
}
