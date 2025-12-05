using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "create-calculated-attribute-definition")]
public record AwsCustomerProfilesCreateCalculatedAttributeDefinitionOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--calculated-attribute-name")] string CalculatedAttributeName,
[property: CliOption("--attribute-details")] string AttributeDetails,
[property: CliOption("--statistic")] string Statistic
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--conditions")]
    public string? Conditions { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}