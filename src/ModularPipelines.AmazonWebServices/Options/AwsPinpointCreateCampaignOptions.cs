using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-campaign")]
public record AwsPinpointCreateCampaignOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--write-campaign-request")] string WriteCampaignRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}