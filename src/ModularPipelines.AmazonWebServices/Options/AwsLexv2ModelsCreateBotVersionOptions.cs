using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-bot-version")]
public record AwsLexv2ModelsCreateBotVersionOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version-locale-specification")] IEnumerable<KeyValue> BotVersionLocaleSpecification
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}