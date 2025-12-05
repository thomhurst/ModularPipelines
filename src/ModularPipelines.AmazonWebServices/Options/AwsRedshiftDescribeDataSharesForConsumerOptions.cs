using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-data-shares-for-consumer")]
public record AwsRedshiftDescribeDataSharesForConsumerOptions : AwsOptions
{
    [CliOption("--consumer-arn")]
    public string? ConsumerArn { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}