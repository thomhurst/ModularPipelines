using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "update-notification-settings")]
public record AwsMturkUpdateNotificationSettingsOptions(
[property: CliOption("--hit-type-id")] string HitTypeId
) : AwsOptions
{
    [CliOption("--notification")]
    public string? Notification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}