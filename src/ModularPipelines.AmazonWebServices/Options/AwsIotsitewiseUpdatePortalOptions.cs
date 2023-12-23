using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-portal")]
public record AwsIotsitewiseUpdatePortalOptions(
[property: CommandSwitch("--portal-id")] string PortalId,
[property: CommandSwitch("--portal-name")] string PortalName,
[property: CommandSwitch("--portal-contact-email")] string PortalContactEmail,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--portal-description")]
    public string? PortalDescription { get; set; }

    [CommandSwitch("--portal-logo-image")]
    public string? PortalLogoImage { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--notification-sender-email")]
    public string? NotificationSenderEmail { get; set; }

    [CommandSwitch("--alarms")]
    public string? Alarms { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}