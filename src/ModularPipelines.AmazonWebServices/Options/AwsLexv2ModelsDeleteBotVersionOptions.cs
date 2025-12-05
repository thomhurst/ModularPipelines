using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "delete-bot-version")]
public record AwsLexv2ModelsDeleteBotVersionOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}