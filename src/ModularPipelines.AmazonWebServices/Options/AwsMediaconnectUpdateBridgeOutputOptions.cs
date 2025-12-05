using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-bridge-output")]
public record AwsMediaconnectUpdateBridgeOutputOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--output-name")] string OutputName
) : AwsOptions
{
    [CliOption("--network-output")]
    public string? NetworkOutput { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}