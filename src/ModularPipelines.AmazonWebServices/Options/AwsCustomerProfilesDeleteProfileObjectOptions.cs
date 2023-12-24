using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "delete-profile-object")]
public record AwsCustomerProfilesDeleteProfileObjectOptions(
[property: CommandSwitch("--profile-id")] string ProfileId,
[property: CommandSwitch("--profile-object-unique-key")] string ProfileObjectUniqueKey,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}