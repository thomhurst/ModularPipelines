using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("translate", "translate-text")]
public record AwsTranslateTranslateTextOptions(
[property: CommandSwitch("--text")] string Text,
[property: CommandSwitch("--source-language-code")] string SourceLanguageCode,
[property: CommandSwitch("--target-language-code")] string TargetLanguageCode
) : AwsOptions
{
    [CommandSwitch("--terminology-names")]
    public string[]? TerminologyNames { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}