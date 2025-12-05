using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-calculated-attribute-definition")]
public record AwsCustomerProfilesDeleteCalculatedAttributeDefinitionOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--calculated-attribute-name")] string CalculatedAttributeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}