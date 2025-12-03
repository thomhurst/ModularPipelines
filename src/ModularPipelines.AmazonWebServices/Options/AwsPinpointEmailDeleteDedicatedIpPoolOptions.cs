using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "delete-dedicated-ip-pool")]
public record AwsPinpointEmailDeleteDedicatedIpPoolOptions(
[property: CliOption("--pool-name")] string PoolName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}