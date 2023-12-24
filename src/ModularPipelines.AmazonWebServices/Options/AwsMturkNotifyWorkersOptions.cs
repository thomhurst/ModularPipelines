using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "notify-workers")]
public record AwsMturkNotifyWorkersOptions(
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--message-text")] string MessageText,
[property: CommandSwitch("--worker-ids")] string[] WorkerIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}