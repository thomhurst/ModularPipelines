using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "settings", "update")]
public record GcloudAccessApprovalSettingsUpdateOptions : GcloudOptions
{
    [CommandSwitch("--active_key_version")]
    public string? ActiveKeyVersion { get; set; }

    [CommandSwitch("--enrolled_services")]
    public string? EnrolledServices { get; set; }

    [CommandSwitch("--notification_emails")]
    public string? NotificationEmails { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}