using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "list-sql-injection-match-sets")]
public record AwsWafRegionalListSqlInjectionMatchSetsOptions : AwsOptions
{
    [CliOption("--next-marker")]
    public string? NextMarker { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}