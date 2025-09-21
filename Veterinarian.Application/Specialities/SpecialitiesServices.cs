using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Sécialities;
using Veterinarian.Application.UnitsOfWork;

namespace Veterinarian.Application.Specialities
{
    public class SpecialitiesServices : ISpecialitiesServices
    {
        private readonly SpecialitiesUnitOfWork _specialitiesUnit;
        private readonly ILogger<SpecialitiesServices> _logger;

        public SpecialitiesServices(SpecialitiesUnitOfWork specialitiesUnit,
            ILogger<SpecialitiesServices> logger)
        {
            _specialitiesUnit = specialitiesUnit;
            _logger = logger;
        }
        public async Task<Result> CreateAsync(SpecialitiesRequest request)
        {

            var speciality = new Speciality()
            {
                Name = request.Name,

            };

            var specialityAdded = await _specialitiesUnit.SpecialityRepository.AddAsync(speciality);
            await _specialitiesUnit.SaveChangesAsync();

            return Result.Success(specialityAdded);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var specliatiy = await _specialitiesUnit.SpecialityRepository.GetByIdAsync(id);

            if(specliatiy is null )
            {
                return Result.Failure(SpecialityError.SpecialityNotFound);
            }

            _specialitiesUnit.SpecialityRepository.Delete(specliatiy!);
            await _specialitiesUnit.SaveChangesAsync();

            return Result.Success();


        }

        public async Task<Result<List<SpecialitiesResponse>>> GetAllAsync()
        {
            var specialities = await _specialitiesUnit.SpecialityRepository.GetAllAsync();

            var specialitiesResponse = specialities.Select(s => new SpecialitiesResponse
            {
                Name = s.Name,
            }).ToList();

            if(specialitiesResponse is null || specialitiesResponse.Count <= 0)
            {
                return Result.Success(new List<SpecialitiesResponse>());
            }

            return Result.Success(specialitiesResponse);

        }

        public async Task<Result<SpecialitiesResponse>> GetByIdAsync(Guid id)
        {
            var scpeciality = await _specialitiesUnit.SpecialityRepository.GetByIdAsync(id);
            if(scpeciality is null)
            {
                return Result.Failure<SpecialitiesResponse>(SpecialityError.SpecialityNotFound);
            }
            var specialityResponse = new SpecialitiesResponse
            {
                Name = scpeciality.Name
            };

            return Result.Success(specialityResponse);

        }

        public async Task<Result> UpdateAsync(Guid id, SpecialitiesRequest resources)
        {
            var speciality = await _specialitiesUnit.SpecialityRepository.GetByIdAsync(id);

            if(speciality is null)
            {
                return Result.Failure(SpecialityError.SpecialityNotFound);
            }

            speciality.Name = resources.Name;

            _specialitiesUnit.SpecialityRepository.UpdateAsync(speciality);

            await _specialitiesUnit.SaveChangesAsync();
            return Result.Success();


        }
    }
}
