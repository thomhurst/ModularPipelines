using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-subscription-request")]
public record AwsDatazoneCreateSubscriptionRequestOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--request-reason")] string RequestReason,
[property: CliOption("--subscribed-listings")] string[] SubscribedListings,
[property: CliOption("--subscribed-principals")] string[] SubscribedPrincipals
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}