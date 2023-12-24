using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-user-subscriptions", "update-identity-provider-settings")]
public record AwsLicenseManagerUserSubscriptionsUpdateIdentityProviderSettingsOptions(
[property: CommandSwitch("--identity-provider")] string IdentityProvider,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--update-settings")] string UpdateSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}