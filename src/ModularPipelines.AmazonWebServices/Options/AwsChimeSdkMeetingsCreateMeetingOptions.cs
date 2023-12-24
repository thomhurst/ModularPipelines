using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "create-meeting")]
public record AwsChimeSdkMeetingsCreateMeetingOptions(
[property: CommandSwitch("--media-region")] string MediaRegion,
[property: CommandSwitch("--external-meeting-id")] string ExternalMeetingId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--meeting-host-id")]
    public string? MeetingHostId { get; set; }

    [CommandSwitch("--notifications-configuration")]
    public string? NotificationsConfiguration { get; set; }

    [CommandSwitch("--meeting-features")]
    public string? MeetingFeatures { get; set; }

    [CommandSwitch("--primary-meeting-id")]
    public string? PrimaryMeetingId { get; set; }

    [CommandSwitch("--tenant-ids")]
    public string[]? TenantIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}