using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-intent-paths")]
public record AwsLexv2ModelsListIntentPathsOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--start-date-time")] long StartDateTime,
[property: CommandSwitch("--end-date-time")] long EndDateTime,
[property: CommandSwitch("--intent-path")] string IntentPath
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}