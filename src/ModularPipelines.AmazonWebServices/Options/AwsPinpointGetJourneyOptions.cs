using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-journey")]
public record AwsPinpointGetJourneyOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--journey-id")] string JourneyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}