using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-detector-version")]
public record AwsFrauddetectorGetDetectorVersionOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--detector-version-id")] string DetectorVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}