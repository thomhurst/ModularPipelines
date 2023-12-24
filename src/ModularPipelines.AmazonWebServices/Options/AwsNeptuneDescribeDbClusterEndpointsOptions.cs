using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "describe-db-cluster-endpoints")]
public record AwsNeptuneDescribeDbClusterEndpointsOptions : AwsOptions
{
    [CommandSwitch("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CommandSwitch("--db-cluster-endpoint-identifier")]
    public string? DbClusterEndpointIdentifier { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}