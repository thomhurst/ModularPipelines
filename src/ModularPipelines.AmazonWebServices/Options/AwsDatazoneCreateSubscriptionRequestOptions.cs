using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-subscription-request")]
public record AwsDatazoneCreateSubscriptionRequestOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--request-reason")] string RequestReason,
[property: CommandSwitch("--subscribed-listings")] string[] SubscribedListings,
[property: CommandSwitch("--subscribed-principals")] string[] SubscribedPrincipals
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}