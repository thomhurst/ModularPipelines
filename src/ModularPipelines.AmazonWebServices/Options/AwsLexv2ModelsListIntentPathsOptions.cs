using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-intent-paths")]
public record AwsLexv2ModelsListIntentPathsOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--start-date-time")] long StartDateTime,
[property: CliOption("--end-date-time")] long EndDateTime,
[property: CliOption("--intent-path")] string IntentPath
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}