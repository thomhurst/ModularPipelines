using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "policy", "show")]
public record AzBillingPolicyShowOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--customer-name")]
    public string? CustomerName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}