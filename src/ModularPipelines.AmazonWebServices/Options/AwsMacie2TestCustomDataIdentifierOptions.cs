using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "test-custom-data-identifier")]
public record AwsMacie2TestCustomDataIdentifierOptions(
[property: CommandSwitch("--regex")] string Regex,
[property: CommandSwitch("--sample-text")] string SampleText
) : AwsOptions
{
    [CommandSwitch("--ignore-words")]
    public string[]? IgnoreWords { get; set; }

    [CommandSwitch("--keywords")]
    public string[]? Keywords { get; set; }

    [CommandSwitch("--maximum-match-distance")]
    public int? MaximumMatchDistance { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}