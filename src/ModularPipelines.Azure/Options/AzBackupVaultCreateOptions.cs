using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "vault", "create")]
public record AzBackupVaultCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [CliOption("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CliOption("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CliOption("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}