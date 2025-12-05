using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "get-regex-match-set")]
public record AwsWafGetRegexMatchSetOptions(
[property: CliOption("--regex-match-set-id")] string RegexMatchSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}