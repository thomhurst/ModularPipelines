using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-calculated-attribute-for-profile")]
public record AwsCustomerProfilesGetCalculatedAttributeForProfileOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--profile-id")] string ProfileId,
[property: CommandSwitch("--calculated-attribute-name")] string CalculatedAttributeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}