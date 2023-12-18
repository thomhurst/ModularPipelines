using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "account", "update")]
public record AzBillingAccountUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--billing-profiles-value")]
    public string? BillingProfilesValue { get; set; }

    [CommandSwitch("--departments")]
    public string? Departments { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--enrollment-accounts")]
    public int? EnrollmentAccounts { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sold-to")]
    public string? SoldTo { get; set; }
}