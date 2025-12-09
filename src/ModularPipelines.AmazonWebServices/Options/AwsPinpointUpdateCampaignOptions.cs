using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-campaign")]
public record AwsPinpointUpdateCampaignOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--campaign-id")] string CampaignId,
[property: CliOption("--write-campaign-request")] string WriteCampaignRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}