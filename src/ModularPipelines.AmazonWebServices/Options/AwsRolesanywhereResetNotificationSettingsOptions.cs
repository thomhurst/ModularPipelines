using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "reset-notification-settings")]
public record AwsRolesanywhereResetNotificationSettingsOptions(
[property: CliOption("--notification-setting-keys")] string[] NotificationSettingKeys,
[property: CliOption("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}