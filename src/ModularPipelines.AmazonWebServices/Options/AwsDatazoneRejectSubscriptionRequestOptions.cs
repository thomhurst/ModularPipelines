using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "reject-subscription-request")]
public record AwsDatazoneRejectSubscriptionRequestOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--decision-comment")]
    public string? DecisionComment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}