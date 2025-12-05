using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-data", "list-tables")]
public record AwsRedshiftDataListTablesOptions(
[property: CliOption("--database")] string Database
) : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--connected-database")]
    public string? ConnectedDatabase { get; set; }

    [CliOption("--db-user")]
    public string? DbUser { get; set; }

    [CliOption("--schema-pattern")]
    public string? SchemaPattern { get; set; }

    [CliOption("--secret-arn")]
    public string? SecretArn { get; set; }

    [CliOption("--table-pattern")]
    public string? TablePattern { get; set; }

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