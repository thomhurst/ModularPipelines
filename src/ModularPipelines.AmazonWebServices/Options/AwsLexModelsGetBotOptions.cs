using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "get-bot")]
public record AwsLexModelsGetBotOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version-or-alias")] string VersionOrAlias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}