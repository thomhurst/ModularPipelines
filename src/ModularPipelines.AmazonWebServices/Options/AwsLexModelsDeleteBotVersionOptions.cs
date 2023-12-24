using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "delete-bot-version")]
public record AwsLexModelsDeleteBotVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--bot-version")] string BotVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}