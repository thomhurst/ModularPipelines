using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-portal")]
public record AwsIotsitewiseUpdatePortalOptions(
[property: CliOption("--portal-id")] string PortalId,
[property: CliOption("--portal-name")] string PortalName,
[property: CliOption("--portal-contact-email")] string PortalContactEmail,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--portal-description")]
    public string? PortalDescription { get; set; }

    [CliOption("--portal-logo-image")]
    public string? PortalLogoImage { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--notification-sender-email")]
    public string? NotificationSenderEmail { get; set; }

    [CliOption("--alarms")]
    public string? Alarms { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}