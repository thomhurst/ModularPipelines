using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "role-definition", "show")]
public record AzBillingRoleDefinitionShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}