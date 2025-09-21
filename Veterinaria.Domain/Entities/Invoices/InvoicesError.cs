using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.Invoices
{
    public static class InvoicesError
    {
        public static Error InvoiceNotFound => new Error("Invoice.NotFound", $"Te invoince was not found");
    }
}
