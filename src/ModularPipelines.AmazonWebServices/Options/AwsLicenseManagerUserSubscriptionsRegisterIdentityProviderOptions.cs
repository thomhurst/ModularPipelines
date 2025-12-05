using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-user-subscriptions", "register-identity-provider")]
public record AwsLicenseManagerUserSubscriptionsRegisterIdentityProviderOptions(
[property: CliOption("--identity-provider")] string IdentityProvider,
[property: CliOption("--product")] string Product
) : AwsOptions
{
    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}