using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-dedicated-ip-warmup-attributes")]
public record AwsSesv2PutDedicatedIpWarmupAttributesOptions(
[property: CliOption("--ip")] string Ip,
[property: CliOption("--warmup-percentage")] int WarmupPercentage
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}