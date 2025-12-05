using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "test-custom-data-identifier")]
public record AwsMacie2TestCustomDataIdentifierOptions(
[property: CliOption("--regex")] string Regex,
[property: CliOption("--sample-text")] string SampleText
) : AwsOptions
{
    [CliOption("--ignore-words")]
    public string[]? IgnoreWords { get; set; }

    [CliOption("--keywords")]
    public string[]? Keywords { get; set; }

    [CliOption("--maximum-match-distance")]
    public int? MaximumMatchDistance { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}