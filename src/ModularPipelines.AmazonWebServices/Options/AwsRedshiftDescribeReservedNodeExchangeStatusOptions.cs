using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-reserved-node-exchange-status")]
public record AwsRedshiftDescribeReservedNodeExchangeStatusOptions : AwsOptions
{
    [CommandSwitch("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CommandSwitch("--reserved-node-exchange-request-id")]
    public string? ReservedNodeExchangeRequestId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}