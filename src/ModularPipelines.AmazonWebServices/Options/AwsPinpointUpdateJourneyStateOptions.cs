using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-journey-state")]
public record AwsPinpointUpdateJourneyStateOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--journey-id")] string JourneyId,
[property: CommandSwitch("--journey-state-request")] string JourneyStateRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}