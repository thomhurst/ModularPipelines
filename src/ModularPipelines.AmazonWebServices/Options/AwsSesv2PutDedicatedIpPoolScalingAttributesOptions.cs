using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-dedicated-ip-pool-scaling-attributes")]
public record AwsSesv2PutDedicatedIpPoolScalingAttributesOptions(
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--scaling-mode")] string ScalingMode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}