using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "notify-application-state")]
public record AwsMghNotifyApplicationStateOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--update-date-time")]
    public long? UpdateDateTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}