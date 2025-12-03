using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "restore")]
public record GcloudAlloydbClustersRestoreOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--region")] string Region,
[property: CliOption("--backup")] string Backup,
[property: CliOption("--point-in-time")] string PointInTime,
[property: CliOption("--source-cluster")] string SourceCluster
) : GcloudOptions
{
    [CliOption("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}