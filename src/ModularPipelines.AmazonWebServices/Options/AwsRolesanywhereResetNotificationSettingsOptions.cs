using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "reset-notification-settings")]
public record AwsRolesanywhereResetNotificationSettingsOptions(
[property: CommandSwitch("--notification-setting-keys")] string[] NotificationSettingKeys,
[property: CommandSwitch("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}