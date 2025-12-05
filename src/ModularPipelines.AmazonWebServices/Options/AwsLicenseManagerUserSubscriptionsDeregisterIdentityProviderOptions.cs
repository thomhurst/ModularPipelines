using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-user-subscriptions", "deregister-identity-provider")]
public record AwsLicenseManagerUserSubscriptionsDeregisterIdentityProviderOptions(
[property: CliOption("--identity-provider")] string IdentityProvider,
[property: CliOption("--product")] string Product
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}