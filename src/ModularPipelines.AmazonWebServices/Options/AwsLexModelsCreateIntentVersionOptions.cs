using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "create-intent-version")]
public record AwsLexModelsCreateIntentVersionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}