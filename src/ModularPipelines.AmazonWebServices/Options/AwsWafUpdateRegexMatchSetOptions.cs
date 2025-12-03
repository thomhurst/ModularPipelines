using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-regex-match-set")]
public record AwsWafUpdateRegexMatchSetOptions(
[property: CliOption("--regex-match-set-id")] string RegexMatchSetId,
[property: CliOption("--updates")] string[] Updates,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}