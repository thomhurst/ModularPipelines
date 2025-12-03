using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-instance-automated-backups")]
public record AwsRdsDescribeDbInstanceAutomatedBackupsOptions : AwsOptions
{
    [CliOption("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CliOption("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--db-instance-automated-backups-arn")]
    public string? DbInstanceAutomatedBackupsArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}