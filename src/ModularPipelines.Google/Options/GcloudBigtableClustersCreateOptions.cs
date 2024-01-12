using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "clusters", "create")]
public record GcloudBigtableClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--num-nodes")]
    public string? NumNodes { get; set; }

    [CommandSwitch("--autoscaling-cpu-target")]
    public string? AutoscalingCpuTarget { get; set; }

    [CommandSwitch("--autoscaling-max-nodes")]
    public string? AutoscalingMaxNodes { get; set; }

    [CommandSwitch("--autoscaling-min-nodes")]
    public string? AutoscalingMinNodes { get; set; }

    [CommandSwitch("--autoscaling-storage-target")]
    public string? AutoscalingStorageTarget { get; set; }
}