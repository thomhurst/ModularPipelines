using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "get-domain-deliverability-campaign")]
public record AwsSesv2GetDomainDeliverabilityCampaignOptions(
[property: CliOption("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}