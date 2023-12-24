using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "create-language-model")]
public record AwsTranscribeCreateLanguageModelOptions(
[property: CommandSwitch("--language-code")] string LanguageCode,
[property: CommandSwitch("--base-model-name")] string BaseModelName,
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--input-data-config")] string InputDataConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}