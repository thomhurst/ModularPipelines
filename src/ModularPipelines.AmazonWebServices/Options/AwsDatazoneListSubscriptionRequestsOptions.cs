using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "list-subscription-requests")]
public record AwsDatazoneListSubscriptionRequestsOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CommandSwitch("--approver-project-id")]
    public string? ApproverProjectId { get; set; }

    [CommandSwitch("--owning-project-id")]
    public string? OwningProjectId { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscribed-listing-id")]
    public string? SubscribedListingId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}