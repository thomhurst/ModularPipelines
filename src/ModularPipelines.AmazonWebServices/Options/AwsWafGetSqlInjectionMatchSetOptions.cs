using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "get-sql-injection-match-set")]
public record AwsWafGetSqlInjectionMatchSetOptions(
[property: CliOption("--sql-injection-match-set-id")] string SqlInjectionMatchSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}