using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "update-calculated-attribute-definition")]
public record AwsCustomerProfilesUpdateCalculatedAttributeDefinitionOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--calculated-attribute-name")] string CalculatedAttributeName
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--conditions")]
    public string? Conditions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}