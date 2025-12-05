using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "start-import")]
public record AwsLexModelsStartImportOptions(
[property: CliOption("--payload")] string Payload,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--merge-strategy")] string MergeStrategy
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}