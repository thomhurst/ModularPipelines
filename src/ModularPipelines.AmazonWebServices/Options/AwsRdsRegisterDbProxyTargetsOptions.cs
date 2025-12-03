using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "register-db-proxy-targets")]
public record AwsRdsRegisterDbProxyTargetsOptions(
[property: CliOption("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CliOption("--target-group-name")]
    public string? TargetGroupName { get; set; }

    [CliOption("--db-instance-identifiers")]
    public string[]? DbInstanceIdentifiers { get; set; }

    [CliOption("--db-cluster-identifiers")]
    public string[]? DbClusterIdentifiers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}