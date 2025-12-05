using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "create-regex-pattern-set")]
public record AwsWafv2CreateRegexPatternSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--regular-expression-list")] string[] RegularExpressionList
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}