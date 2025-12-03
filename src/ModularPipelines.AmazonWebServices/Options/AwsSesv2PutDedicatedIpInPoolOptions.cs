using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-dedicated-ip-in-pool")]
public record AwsSesv2PutDedicatedIpInPoolOptions(
[property: CliOption("--ip")] string Ip,
[property: CliOption("--destination-pool-name")] string DestinationPoolName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}