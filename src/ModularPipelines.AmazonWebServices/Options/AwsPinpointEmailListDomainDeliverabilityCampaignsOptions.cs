using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "list-domain-deliverability-campaigns")]
public record AwsPinpointEmailListDomainDeliverabilityCampaignsOptions(
[property: CommandSwitch("--start-date")] long StartDate,
[property: CommandSwitch("--end-date")] long EndDate,
[property: CommandSwitch("--subscribed-domain")] string SubscribedDomain
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}