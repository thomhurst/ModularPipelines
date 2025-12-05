using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "create-meeting")]
public record AwsChimeSdkMeetingsCreateMeetingOptions(
[property: CliOption("--media-region")] string MediaRegion,
[property: CliOption("--external-meeting-id")] string ExternalMeetingId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--meeting-host-id")]
    public string? MeetingHostId { get; set; }

    [CliOption("--notifications-configuration")]
    public string? NotificationsConfiguration { get; set; }

    [CliOption("--meeting-features")]
    public string? MeetingFeatures { get; set; }

    [CliOption("--primary-meeting-id")]
    public string? PrimaryMeetingId { get; set; }

    [CliOption("--tenant-ids")]
    public string[]? TenantIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}