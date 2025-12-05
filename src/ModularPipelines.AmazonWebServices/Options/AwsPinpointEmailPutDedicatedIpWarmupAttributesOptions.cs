using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-dedicated-ip-warmup-attributes")]
public record AwsPinpointEmailPutDedicatedIpWarmupAttributesOptions(
[property: CliOption("--ip")] string Ip,
[property: CliOption("--warmup-percentage")] int WarmupPercentage
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}