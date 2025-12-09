using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-proxy-target-group")]
public record AwsRdsModifyDbProxyTargetGroupOptions(
[property: CliOption("--target-group-name")] string TargetGroupName,
[property: CliOption("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CliOption("--connection-pool-config")]
    public string? ConnectionPoolConfig { get; set; }

    [CliOption("--new-name")]
    public string? NewName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}