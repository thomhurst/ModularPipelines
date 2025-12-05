using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "delete-detector-version")]
public record AwsFrauddetectorDeleteDetectorVersionOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--detector-version-id")] string DetectorVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}