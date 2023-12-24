using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "delete-profile-object-type")]
public record AwsCustomerProfilesDeleteProfileObjectTypeOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}