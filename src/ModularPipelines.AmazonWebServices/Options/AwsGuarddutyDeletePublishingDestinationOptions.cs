using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "delete-publishing-destination")]
public record AwsGuarddutyDeletePublishingDestinationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--destination-id")] string DestinationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}