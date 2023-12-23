using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-user-subscriptions", "stop-product-subscription")]
public record AwsLicenseManagerUserSubscriptionsStopProductSubscriptionOptions(
[property: CommandSwitch("--identity-provider")] string IdentityProvider,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}