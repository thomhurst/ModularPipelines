using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-detector-version-metadata")]
public record AwsFrauddetectorUpdateDetectorVersionMetadataOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--detector-version-id")] string DetectorVersionId,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}