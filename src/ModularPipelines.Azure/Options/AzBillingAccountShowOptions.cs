using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "account", "show")]
public record AzBillingAccountShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}