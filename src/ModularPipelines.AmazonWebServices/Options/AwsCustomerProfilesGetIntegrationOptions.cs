using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "get-integration")]
public record AwsCustomerProfilesGetIntegrationOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--uri")] string Uri
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}