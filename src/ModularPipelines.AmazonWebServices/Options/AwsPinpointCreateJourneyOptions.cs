using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-journey")]
public record AwsPinpointCreateJourneyOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--write-journey-request")] string WriteJourneyRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}