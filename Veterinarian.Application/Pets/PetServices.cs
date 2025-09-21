using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Application.Common;
using Veterinarian.Application.UnitsOfWork;

namespace Veterinarian.Application.Pets
{
    public class PetServices : IPetServices
    {
        private readonly PetsUnitOfWork _petsUnitOfWork;

        public PetServices(PetsUnitOfWork petsUnitOfWork)
        {
            _petsUnitOfWork = petsUnitOfWork;
        }

        public async Task<Result> CreateAsync(PetRequest request)
        {
            var pet = new Pet
            {
                Name = request.Name,
                Specie = request.Specie,
                Breed = request.Breed,
                GenderStatus = request.GenderStatus,
                BirhtDate = request.BirhtDate,
                Owners = request.Owners?.Select(o => new Owner
                {
                    GivenName = o.GivenName,
                    FamilyName = o.FamilyName,
                    Contact = o.Contact,
                    Email = o.Email,
                }).ToList() ?? new List<Owner>()
            };

            
            await _petsUnitOfWork.PetsRepository.AddAsync(pet);
            await _petsUnitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid Id)
        {
            var pet = await _petsUnitOfWork.PetsRepository.GetByIdAsync(Id);

            if(pet is null)
            {
                return Result.Failure(PetError.PetNotFound);
            }


            _petsUnitOfWork.PetsRepository.Delete(pet);
            await _petsUnitOfWork.SaveChangesAsync();
            return Result.Success(pet);
        }

        public async Task<Result<List<PetResponse>>> GetAllAsync()
        {

            var pets = await _petsUnitOfWork.PetsRepository.GetAllAsync();



            var petsResponse = pets.Select(p => new PetResponse
            {
                Name = p.Name,
                Specie = p.Specie,
                Breed = p.Breed,
                BirhtDate = p.BirhtDate,
                GenderStatus = p.GenderStatus
            }).ToList();



            return Result.Success(petsResponse);

        }

        public async Task<Result<PetResponse>> GetByIdAsync(Guid Id)
        {


            var pet = await _petsUnitOfWork.PetsRepository.GetByIdAsync(Id);
            if(pet is null)
            {
                return Result.Failure<PetResponse>(PetError.PetNotFound);
            }


            var petResponse = new PetResponse
            {
                Name = pet.Name,
                Specie = pet.Specie,
                Breed = pet.Breed,
                GenderStatus = pet.GenderStatus,
                BirhtDate = pet.BirhtDate
            };

            return Result.Success(petResponse);

        }

        public async Task<Result<List<PetResponse>>> SearchPetAsync(string? search)
        {
            var pets =await _petsUnitOfWork.PetsRepository.SearchPetAsync(search);

            var petsResponse = pets.Select(p => new PetResponse
            {
                Name = p.Name,
                Specie = p.Specie,
                Breed = p.Breed,
                BirhtDate = p.BirhtDate,
                GenderStatus = p.GenderStatus,
                Owners = p.Owners.Select(o => new OwnersResources
                {
                    GivenName = o.GivenName,
                    FamilyName = o.FamilyName,
                    Contact = o.Contact,
                    Email = o.Email,
                }).ToList()
            }).ToList();

            if(petsResponse is null || petsResponse.Count <= 0)
            {
                return Result.Success(new List<PetResponse>());
            }

            return Result.Success(petsResponse);


        }

        public async Task<Result<PaginationResultDto<PetResponse>>> SearchPetAsync(string? search, int page = 1, int pageSize = 5)
        {
            var pets = await _petsUnitOfWork.PetsRepository.SearchPetAsync(search,page,pageSize);

            if (pets.Items is null || pets.Items.Count <= 0)
            {
                return Result.Success(new PaginationResultDto<PetResponse>());
            }

            var petsResponse = pets.Items.Select(p => new PetResponse
            {
                Name = p.Name,
                Specie = p.Specie,
                Breed = p.Breed,
                BirhtDate = p.BirhtDate,
                GenderStatus = p.GenderStatus,
                Owners = p.Owners.Select(o => new OwnersResources
                {
                    GivenName = o.GivenName,
                    FamilyName = o.FamilyName,
                    Contact = o.Contact,
                    Email = o.Email,
                }).ToList()
            }).ToList();

            var response = new PaginationResultDto<PetResponse>()
            {
                Items = petsResponse,
                Page = pets.Page,
                PageSize = pets.PageSize,
                TotalCount =  pets.TotalCount

            };

            return Result.Success(response);


        }

        public async Task<Result> UpdateAsync(Guid Id, PetUpdateRequest resources)
        {
            var pet = await _petsUnitOfWork.PetsRepository.GetByIdAsync(Id);

            if(pet is null)
            {
                return Result.Failure(PetError.PetNotFound);
            }

            pet.Name = resources.Name;
            pet.Breed = resources.Breed;
            pet.Specie = resources.Specie;
            pet.GenderStatus = resources.GenderStatus;
            pet.BirhtDate = resources.BirhtDate;

            _petsUnitOfWork.PetsRepository.UpdateAsync(pet);
            await _petsUnitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
