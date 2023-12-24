using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-user-subscriptions", "register-identity-provider")]
public record AwsLicenseManagerUserSubscriptionsRegisterIdentityProviderOptions(
[property: CommandSwitch("--identity-provider")] string IdentityProvider,
[property: CommandSwitch("--product")] string Product
) : AwsOptions
{
    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}