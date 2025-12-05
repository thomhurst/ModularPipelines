using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "get-domain-deliverability-campaign")]
public record AwsPinpointEmailGetDomainDeliverabilityCampaignOptions(
[property: CliOption("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}