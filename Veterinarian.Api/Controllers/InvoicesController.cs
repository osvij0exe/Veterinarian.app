using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.Invoices;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesServices _invoicesServices;
        private readonly IUserContext _userContext;

        public InvoicesController(IInvoicesServices invoicesServices,
            IUserContext userContext)
        {
            _invoicesServices = invoicesServices;
            _userContext = userContext;
        }

        [Authorize(Roles =$"{Role.Admin}")]
        [HttpGet("getAllInvoices")]
        public async Task<IActionResult> GetAllInvoices(CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var invoices = await _invoicesServices.GetAllAsync();
            return Ok(invoices.Value);
        }

        [Authorize(Roles =$"{Role.Admin}")]
        [HttpGet("searchInvoices")]
        public async Task<IActionResult> SearchInvoices(string? search,CancellationToken cancellationToken, int page = 1 , int pageSize = 5)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var invoices = await _invoicesServices.SearchAsync(search,page,pageSize);
            return Ok(invoices.Value);
        }


        [Authorize(Roles =$"{Role.Admin}")]
        [HttpGet("getInvoice/{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var invoice = await _invoicesServices.GetByIdAsync(id);
            return invoice.IsSuccess ? Ok(invoice.Value) : NotFound(invoice.Error);
        }

        [Authorize(Roles = $"{Role.Admin}")]
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoicesRequest request,
            IValidator<InvoicesRequest> validator,CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }


            var result = await _invoicesServices.CreateAsync(request);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var result = await _invoicesServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] InvoicesRequest request,
            IValidator<InvoicesRequest> validator,CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var result = await _invoicesServices.UpdateAsync(id, request);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }
    }
}
