using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-bridge-output")]
public record AwsMediaconnectRemoveBridgeOutputOptions(
[property: CliOption("--bridge-arn")] string BridgeArn,
[property: CliOption("--output-name")] string OutputName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}