using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "list-available-resource-metrics")]
public record AwsPiListAvailableResourceMetricsOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--metric-types")] string[] MetricTypes
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}