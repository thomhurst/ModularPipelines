using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-user-subscriptions", "disassociate-user")]
public record AwsLicenseManagerUserSubscriptionsDisassociateUserOptions(
[property: CommandSwitch("--identity-provider")] string IdentityProvider,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}