using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "profile", "list")]
public record AzBillingProfileListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}