using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "update-notification-settings")]
public record AwsMturkUpdateNotificationSettingsOptions(
[property: CommandSwitch("--hit-type-id")] string HitTypeId
) : AwsOptions
{
    [CommandSwitch("--notification")]
    public string? Notification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}