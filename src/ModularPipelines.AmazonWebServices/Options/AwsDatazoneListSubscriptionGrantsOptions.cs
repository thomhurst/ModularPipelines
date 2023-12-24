using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "list-subscription-grants")]
public record AwsDatazoneListSubscriptionGrantsOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CommandSwitch("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--subscribed-listing-id")]
    public string? SubscribedListingId { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--subscription-target-id")]
    public string? SubscriptionTargetId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}