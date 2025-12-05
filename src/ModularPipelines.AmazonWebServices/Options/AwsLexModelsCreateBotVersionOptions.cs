using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "create-bot-version")]
public record AwsLexModelsCreateBotVersionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}