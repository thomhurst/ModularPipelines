using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-detector")]
public record AwsGuarddutyUpdateDetectorOptions(
[property: CommandSwitch("--detector-id")] string DetectorId
) : AwsOptions
{
    [CommandSwitch("--finding-publishing-frequency")]
    public string? FindingPublishingFrequency { get; set; }

    [CommandSwitch("--data-sources")]
    public string? DataSources { get; set; }

    [CommandSwitch("--features")]
    public string[]? Features { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}