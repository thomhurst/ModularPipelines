using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "cluster", "update")]
public record AzEventhubsClusterUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--supports-scaling")]
    public bool? SupportsScaling { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}