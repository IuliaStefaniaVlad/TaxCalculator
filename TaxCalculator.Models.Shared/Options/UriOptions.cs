using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Models.Shared.Options
{
    public record UriOptions
    {
        [Required]
        [ConfigurationKeyName("BaseUri")]
        public required string BaseUri { get; init; }
    }
}
