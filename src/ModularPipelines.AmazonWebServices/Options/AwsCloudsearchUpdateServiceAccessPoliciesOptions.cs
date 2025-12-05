using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "update-service-access-policies")]
public record AwsCloudsearchUpdateServiceAccessPoliciesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--access-policies")] string AccessPolicies
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}