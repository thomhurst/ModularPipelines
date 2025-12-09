using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "list-subscriptions")]
public record AwsDatazoneListSubscriptionsOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CliOption("--approver-project-id")]
    public string? ApproverProjectId { get; set; }

    [CliOption("--owning-project-id")]
    public string? OwningProjectId { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscribed-listing-id")]
    public string? SubscribedListingId { get; set; }

    [CliOption("--subscription-request-identifier")]
    public string? SubscriptionRequestIdentifier { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}