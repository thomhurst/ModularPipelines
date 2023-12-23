using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("translate", "translate-document")]
public record AwsTranslateTranslateDocumentOptions(
[property: CommandSwitch("--document")] string Document,
[property: CommandSwitch("--source-language-code")] string SourceLanguageCode,
[property: CommandSwitch("--target-language-code")] string TargetLanguageCode,
[property: CommandSwitch("--document-content")] string DocumentContent
) : AwsOptions
{
    [CommandSwitch("--terminology-names")]
    public string[]? TerminologyNames { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}