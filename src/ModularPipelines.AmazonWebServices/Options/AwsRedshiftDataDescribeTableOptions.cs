using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-data", "describe-table")]
public record AwsRedshiftDataDescribeTableOptions(
[property: CliOption("--database")] string Database
) : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--connected-database")]
    public string? ConnectedDatabase { get; set; }

    [CliOption("--db-user")]
    public string? DbUser { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--secret-arn")]
    public string? SecretArn { get; set; }

    [CliOption("--table")]
    public string? Table { get; set; }

    [CliOption("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}