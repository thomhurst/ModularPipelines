using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-campaign-version")]
public record AwsPinpointGetCampaignVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--campaign-id")] string CampaignId,
[property: CliOption("--campaign-version")] string CampaignVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}