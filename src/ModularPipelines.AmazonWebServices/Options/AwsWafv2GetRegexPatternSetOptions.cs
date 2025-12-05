using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-regex-pattern-set")]
public record AwsWafv2GetRegexPatternSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}