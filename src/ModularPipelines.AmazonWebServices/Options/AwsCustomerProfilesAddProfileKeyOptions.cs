using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "add-profile-key")]
public record AwsCustomerProfilesAddProfileKeyOptions(
[property: CommandSwitch("--profile-id")] string ProfileId,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--values")] string[] Values,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}