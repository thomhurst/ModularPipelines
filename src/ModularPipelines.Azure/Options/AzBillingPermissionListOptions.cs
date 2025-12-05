using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "permission", "list")]
public record AzBillingPermissionListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--customer-name")]
    public string? CustomerName { get; set; }

    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}