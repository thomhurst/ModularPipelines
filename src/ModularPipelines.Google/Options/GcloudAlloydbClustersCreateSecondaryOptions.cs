using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "create-secondary")]
public record GcloudAlloydbClustersCreateSecondaryOptions(
[property: PositionalArgument] string Cluster,
[property: CommandSwitch("--primary-cluster")] string PrimaryCluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

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
}