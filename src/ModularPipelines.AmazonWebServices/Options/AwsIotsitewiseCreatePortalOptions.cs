using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-portal")]
public record AwsIotsitewiseCreatePortalOptions(
[property: CliOption("--portal-name")] string PortalName,
[property: CliOption("--portal-contact-email")] string PortalContactEmail,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--portal-description")]
    public string? PortalDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--portal-logo-image-file")]
    public string? PortalLogoImageFile { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--portal-auth-mode")]
    public string? PortalAuthMode { get; set; }

    [CliOption("--notification-sender-email")]
    public string? NotificationSenderEmail { get; set; }

    [CliOption("--alarms")]
    public string? Alarms { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}