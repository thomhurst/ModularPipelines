using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-subscription-request")]
public record AwsDatazoneUpdateSubscriptionRequestOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--request-reason")] string RequestReason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}