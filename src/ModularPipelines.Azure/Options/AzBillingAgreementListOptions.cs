using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "agreement", "list")]
public record AzBillingAgreementListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}