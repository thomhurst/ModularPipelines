using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-user-subscriptions", "update-identity-provider-settings")]
public record AwsLicenseManagerUserSubscriptionsUpdateIdentityProviderSettingsOptions(
[property: CliOption("--identity-provider")] string IdentityProvider,
[property: CliOption("--product")] string Product,
[property: CliOption("--update-settings")] string UpdateSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}