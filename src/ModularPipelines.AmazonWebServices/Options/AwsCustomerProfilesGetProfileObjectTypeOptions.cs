using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-profile-object-type")]
public record AwsCustomerProfilesGetProfileObjectTypeOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}