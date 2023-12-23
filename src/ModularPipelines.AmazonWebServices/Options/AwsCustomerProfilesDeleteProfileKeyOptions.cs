using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "delete-profile-key")]
public record AwsCustomerProfilesDeleteProfileKeyOptions(
[property: CommandSwitch("--profile-id")] string ProfileId,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--values")] string[] Values,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}