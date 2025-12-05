using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-journey")]
public record AwsPinpointUpdateJourneyOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--journey-id")] string JourneyId,
[property: CliOption("--write-journey-request")] string WriteJourneyRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}