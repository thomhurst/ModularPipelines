using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "create-calculated-attribute-definition")]
public record AwsCustomerProfilesCreateCalculatedAttributeDefinitionOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--calculated-attribute-name")] string CalculatedAttributeName,
[property: CommandSwitch("--attribute-details")] string AttributeDetails,
[property: CommandSwitch("--statistic")] string Statistic
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--conditions")]
    public string? Conditions { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}