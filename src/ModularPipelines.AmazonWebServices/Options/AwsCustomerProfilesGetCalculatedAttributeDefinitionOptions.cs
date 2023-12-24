using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-calculated-attribute-definition")]
public record AwsCustomerProfilesGetCalculatedAttributeDefinitionOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--calculated-attribute-name")] string CalculatedAttributeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}