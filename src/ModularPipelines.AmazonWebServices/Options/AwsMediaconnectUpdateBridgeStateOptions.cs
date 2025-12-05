using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-bridge-state")]
public record AwsMediaconnectUpdateBridgeStateOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--desired-state")] string DesiredState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}