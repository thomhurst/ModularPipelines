using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "account", "update")]
public record AzBillingAccountUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--billing-profiles-value")]
    public string? BillingProfilesValue { get; set; }

    [CliOption("--departments")]
    public string? Departments { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--enrollment-accounts")]
    public int? EnrollmentAccounts { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sold-to")]
    public string? SoldTo { get; set; }
}