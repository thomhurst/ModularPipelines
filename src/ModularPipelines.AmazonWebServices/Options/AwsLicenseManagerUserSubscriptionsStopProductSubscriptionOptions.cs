using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-user-subscriptions", "stop-product-subscription")]
public record AwsLicenseManagerUserSubscriptionsStopProductSubscriptionOptions(
[property: CliOption("--identity-provider")] string IdentityProvider,
[property: CliOption("--product")] string Product,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}