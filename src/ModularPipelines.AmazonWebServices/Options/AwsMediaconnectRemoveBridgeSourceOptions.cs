using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-bridge-source")]
public record AwsMediaconnectRemoveBridgeSourceOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--source-name")] string SourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}