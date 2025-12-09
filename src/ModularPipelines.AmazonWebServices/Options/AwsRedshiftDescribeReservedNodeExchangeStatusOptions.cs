using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-reserved-node-exchange-status")]
public record AwsRedshiftDescribeReservedNodeExchangeStatusOptions : AwsOptions
{
    [CliOption("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CliOption("--reserved-node-exchange-request-id")]
    public string? ReservedNodeExchangeRequestId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}