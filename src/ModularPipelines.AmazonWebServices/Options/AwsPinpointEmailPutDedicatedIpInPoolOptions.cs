using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-dedicated-ip-in-pool")]
public record AwsPinpointEmailPutDedicatedIpInPoolOptions(
[property: CliOption("--ip")] string Ip,
[property: CliOption("--destination-pool-name")] string DestinationPoolName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}