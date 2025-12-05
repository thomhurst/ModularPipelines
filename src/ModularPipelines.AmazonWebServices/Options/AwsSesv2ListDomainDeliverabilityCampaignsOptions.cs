using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "list-domain-deliverability-campaigns")]
public record AwsSesv2ListDomainDeliverabilityCampaignsOptions(
[property: CliOption("--start-date")] long StartDate,
[property: CliOption("--end-date")] long EndDate,
[property: CliOption("--subscribed-domain")] string SubscribedDomain
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}