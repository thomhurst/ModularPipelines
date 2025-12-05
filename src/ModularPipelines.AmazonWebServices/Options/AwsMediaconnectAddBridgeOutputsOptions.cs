using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-bridge-outputs")]
public record AwsMediaconnectAddBridgeOutputsOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--outputs")] string[] Outputs
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}