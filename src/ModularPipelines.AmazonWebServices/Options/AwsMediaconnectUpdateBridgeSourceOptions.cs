using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-bridge-source")]
public record AwsMediaconnectUpdateBridgeSourceOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--source-name")] string SourceName
) : AwsOptions
{
    [CliOption("--flow-source")]
    public string? FlowSource { get; set; }

    [CliOption("--network-source")]
    public string? NetworkSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}