using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "create-language-model")]
public record AwsTranscribeCreateLanguageModelOptions(
[property: CliOption("--language-code")] string LanguageCode,
[property: CliOption("--base-model-name")] string BaseModelName,
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--input-data-config")] string InputDataConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}