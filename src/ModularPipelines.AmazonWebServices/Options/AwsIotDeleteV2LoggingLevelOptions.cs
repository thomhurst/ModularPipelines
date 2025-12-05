using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "delete-v2-logging-level")]
public record AwsIotDeleteV2LoggingLevelOptions(
[property: CliOption("--target-type")] string TargetType,
[property: CliOption("--target-name")] string TargetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}