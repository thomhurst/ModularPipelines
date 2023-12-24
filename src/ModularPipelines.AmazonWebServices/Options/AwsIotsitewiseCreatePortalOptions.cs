using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-portal")]
public record AwsIotsitewiseCreatePortalOptions(
[property: CommandSwitch("--portal-name")] string PortalName,
[property: CommandSwitch("--portal-contact-email")] string PortalContactEmail,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--portal-description")]
    public string? PortalDescription { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--portal-logo-image-file")]
    public string? PortalLogoImageFile { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--portal-auth-mode")]
    public string? PortalAuthMode { get; set; }

    [CommandSwitch("--notification-sender-email")]
    public string? NotificationSenderEmail { get; set; }

    [CommandSwitch("--alarms")]
    public string? Alarms { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}