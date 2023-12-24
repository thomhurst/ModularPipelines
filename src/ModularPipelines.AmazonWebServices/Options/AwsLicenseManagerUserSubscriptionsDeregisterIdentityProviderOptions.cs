using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-user-subscriptions", "deregister-identity-provider")]
public record AwsLicenseManagerUserSubscriptionsDeregisterIdentityProviderOptions(
[property: CommandSwitch("--identity-provider")] string IdentityProvider,
[property: CommandSwitch("--product")] string Product
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}