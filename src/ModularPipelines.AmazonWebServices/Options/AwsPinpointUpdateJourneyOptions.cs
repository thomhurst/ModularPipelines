using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-journey")]
public record AwsPinpointUpdateJourneyOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--journey-id")] string JourneyId,
[property: CommandSwitch("--write-journey-request")] string WriteJourneyRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}