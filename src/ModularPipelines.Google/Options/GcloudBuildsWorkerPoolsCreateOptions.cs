using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "worker-pools", "create")]
public record GcloudBuildsWorkerPoolsCreateOptions(
[property: PositionalArgument] string WorkerPool,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--config-from-file")]
    public string? ConfigFromFile { get; set; }

    [CommandSwitch("--peered-network")]
    public string? PeeredNetwork { get; set; }

    [CommandSwitch("--peered-network-ip-range")]
    public string? PeeredNetworkIpRange { get; set; }

    [BooleanCommandSwitch("--no-public-egress")]
    public bool? NoPublicEgress { get; set; }

    [CommandSwitch("--worker-disk-size")]
    public string? WorkerDiskSize { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }
}