using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-bridge-sources")]
public record AwsMediaconnectAddBridgeSourcesOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}