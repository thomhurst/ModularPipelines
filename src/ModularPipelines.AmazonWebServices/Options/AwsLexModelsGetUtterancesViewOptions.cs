using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-utterances-view")]
public record AwsLexModelsGetUtterancesViewOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-versions")] string[] BotVersions,
[property: CommandSwitch("--status-type")] string StatusType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}