using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "update-calculated-attribute-definition")]
public record AwsCustomerProfilesUpdateCalculatedAttributeDefinitionOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--calculated-attribute-name")] string CalculatedAttributeName
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--conditions")]
    public string? Conditions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}