using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "get-snapshots")]
public record AwsKendraGetSnapshotsOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--interval")] string Interval,
[property: CommandSwitch("--metric-type")] string MetricType
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}