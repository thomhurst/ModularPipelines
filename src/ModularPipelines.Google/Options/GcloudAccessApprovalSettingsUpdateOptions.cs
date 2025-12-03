using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-approval", "settings", "update")]
public record GcloudAccessApprovalSettingsUpdateOptions : GcloudOptions
{
    [CliOption("--active_key_version")]
    public string? ActiveKeyVersion { get; set; }

    [CliOption("--enrolled_services")]
    public string? EnrolledServices { get; set; }

    [CliOption("--notification_emails")]
    public string? NotificationEmails { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}