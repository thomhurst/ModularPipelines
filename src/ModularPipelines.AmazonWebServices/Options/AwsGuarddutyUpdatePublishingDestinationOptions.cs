using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-publishing-destination")]
public record AwsGuarddutyUpdatePublishingDestinationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--destination-id")] string DestinationId
) : AwsOptions
{
    [CommandSwitch("--destination-properties")]
    public string? DestinationProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}