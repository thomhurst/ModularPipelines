using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "restore")]
public record GcloudAlloydbClustersRestoreOptions(
[property: PositionalArgument] string Cluster,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--backup")] string Backup,
[property: CommandSwitch("--point-in-time")] string PointInTime,
[property: CommandSwitch("--source-cluster")] string SourceCluster
) : GcloudOptions
{
    [CommandSwitch("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}