using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "get-regex-match-set")]
public record AwsWafRegionalGetRegexMatchSetOptions(
[property: CliOption("--regex-match-set-id")] string RegexMatchSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}