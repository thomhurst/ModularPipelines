using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "get-snapshots")]
public record AwsKendraGetSnapshotsOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--interval")] string Interval,
[property: CliOption("--metric-type")] string MetricType
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}