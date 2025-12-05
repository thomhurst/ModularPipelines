using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-bot-alias")]
public record AwsLexv2ModelsDescribeBotAliasOptions(
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}