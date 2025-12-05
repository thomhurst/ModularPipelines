using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "put-notification-settings")]
public record AwsRolesanywherePutNotificationSettingsOptions(
[property: CliOption("--notification-settings")] string[] NotificationSettings,
[property: CliOption("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}