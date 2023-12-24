using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "put-notification-settings")]
public record AwsRolesanywherePutNotificationSettingsOptions(
[property: CommandSwitch("--notification-settings")] string[] NotificationSettings,
[property: CommandSwitch("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}