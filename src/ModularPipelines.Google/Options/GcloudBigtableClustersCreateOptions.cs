using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "clusters", "create")]
public record GcloudBigtableClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--num-nodes")]
    public string? NumNodes { get; set; }

    [CliOption("--autoscaling-cpu-target")]
    public string? AutoscalingCpuTarget { get; set; }

    [CliOption("--autoscaling-max-nodes")]
    public string? AutoscalingMaxNodes { get; set; }

    [CliOption("--autoscaling-min-nodes")]
    public string? AutoscalingMinNodes { get; set; }

    [CliOption("--autoscaling-storage-target")]
    public string? AutoscalingStorageTarget { get; set; }
}