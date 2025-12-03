using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-detector-version-metadata")]
public record AwsFrauddetectorUpdateDetectorVersionMetadataOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--detector-version-id")] string DetectorVersionId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}