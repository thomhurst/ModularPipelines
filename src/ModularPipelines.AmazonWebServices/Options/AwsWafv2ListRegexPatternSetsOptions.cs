using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "list-regex-pattern-sets")]
public record AwsWafv2ListRegexPatternSetsOptions(
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--next-marker")]
    public string? NextMarker { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}