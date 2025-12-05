using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "delete-dedicated-ip-pool")]
public record AwsSesv2DeleteDedicatedIpPoolOptions(
[property: CliOption("--pool-name")] string PoolName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}