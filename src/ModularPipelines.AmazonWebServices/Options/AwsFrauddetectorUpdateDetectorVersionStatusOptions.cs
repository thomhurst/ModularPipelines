using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-detector-version-status")]
public record AwsFrauddetectorUpdateDetectorVersionStatusOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--detector-version-id")] string DetectorVersionId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}