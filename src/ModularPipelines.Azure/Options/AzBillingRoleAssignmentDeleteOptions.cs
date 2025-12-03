using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "role-assignment", "delete")]
public record AzBillingRoleAssignmentDeleteOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}