using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-runtime", "get-session")]
public record AwsLexRuntimeGetSessionOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-alias")] string BotAlias,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--checkpoint-label-filter")]
    public string? CheckpointLabelFilter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}