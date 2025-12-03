using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "worker-pools", "create")]
public record GcloudBuildsWorkerPoolsCreateOptions(
[property: CliArgument] string WorkerPool,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--config-from-file")]
    public string? ConfigFromFile { get; set; }

    [CliOption("--peered-network")]
    public string? PeeredNetwork { get; set; }

    [CliOption("--peered-network-ip-range")]
    public string? PeeredNetworkIpRange { get; set; }

    [CliFlag("--no-public-egress")]
    public bool? NoPublicEgress { get; set; }

    [CliOption("--worker-disk-size")]
    public string? WorkerDiskSize { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }
}