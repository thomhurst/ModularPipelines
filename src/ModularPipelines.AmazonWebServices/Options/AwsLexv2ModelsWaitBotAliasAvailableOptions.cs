using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "wait", "bot-alias-available")]
public record AwsLexv2ModelsWaitBotAliasAvailableOptions(
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}