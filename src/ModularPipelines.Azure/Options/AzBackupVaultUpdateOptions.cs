using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "vault", "update")]
public record AzBackupVaultUpdateOptions : AzOptions
{
    [CommandSwitch("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [CommandSwitch("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CommandSwitch("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}