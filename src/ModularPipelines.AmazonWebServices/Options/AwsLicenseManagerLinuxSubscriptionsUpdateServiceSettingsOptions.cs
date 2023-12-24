using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager-linux-subscriptions", "update-service-settings")]
public record AwsLicenseManagerLinuxSubscriptionsUpdateServiceSettingsOptions(
[property: CommandSwitch("--linux-subscriptions-discovery")] string LinuxSubscriptionsDiscovery,
[property: CommandSwitch("--linux-subscriptions-discovery-settings")] string LinuxSubscriptionsDiscoverySettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}