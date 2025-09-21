using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Invoices;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesServices _invoicesServices;

        public InvoicesController(IInvoicesServices invoicesServices)
        {
            _invoicesServices = invoicesServices;
        }

        [HttpGet("GetAllInvoices")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoicesServices.GetAllAsync();
            return Ok(invoices.Value);
        }

        [HttpGet("SearchInvoices")]
        public async Task<IActionResult> SearchInvoices(string? search, int page = 1 , int pageSize = 5)
        {
            var invoices = await _invoicesServices.SearchAsync(search,page,pageSize);
            return Ok(invoices.Value);
        }


        [HttpGet("GetInvoice/{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var invoice = await _invoicesServices.GetByIdAsync(id);
            return invoice.IsSuccess ? Ok(invoice.Value) : NotFound(invoice.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoicesRequest request,
            IValidator<InvoicesRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }


            var result = await _invoicesServices.CreateAsync(request);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var result = await _invoicesServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] InvoicesRequest request,
            IValidator<InvoicesRequest> validator)
        {
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
