using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "account", "list")]
public record AzBillingAccountListOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}