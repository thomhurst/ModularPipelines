using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "role-definition", "list")]
public record AzBillingRoleDefinitionListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}