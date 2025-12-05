using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "create-secondary")]
public record GcloudAlloydbClustersCreateSecondaryOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--primary-cluster")] string PrimaryCluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

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
}