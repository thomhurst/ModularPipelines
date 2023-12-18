using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "vault", "create")]
public record AzBackupVaultCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [CommandSwitch("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CommandSwitch("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CommandSwitch("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}