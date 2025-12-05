using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("polly", "put-lexicon")]
public record AwsPollyPutLexiconOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}