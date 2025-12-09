using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-bot-version")]
public record AwsLexv2ModelsCreateBotVersionOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version-locale-specification")] IEnumerable<KeyValue> BotVersionLocaleSpecification
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}