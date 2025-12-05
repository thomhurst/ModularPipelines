using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "delete-campaign")]
public record AwsPinpointDeleteCampaignOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}