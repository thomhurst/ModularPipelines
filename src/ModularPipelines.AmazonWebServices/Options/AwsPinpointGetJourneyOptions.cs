using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-journey")]
public record AwsPinpointGetJourneyOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--journey-id")] string JourneyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}