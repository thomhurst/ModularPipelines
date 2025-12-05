using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager-linux-subscriptions", "update-service-settings")]
public record AwsLicenseManagerLinuxSubscriptionsUpdateServiceSettingsOptions(
[property: CliOption("--linux-subscriptions-discovery")] string LinuxSubscriptionsDiscovery,
[property: CliOption("--linux-subscriptions-discovery-settings")] string LinuxSubscriptionsDiscoverySettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}