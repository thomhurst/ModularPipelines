using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "role-assignment", "list")]
public record AzBillingRoleAssignmentListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}