using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-applicable-individual-assessments")]
public record AwsDmsDescribeApplicableIndividualAssessmentsOptions : AwsOptions
{
    [CliOption("--replication-task-arn")]
    public string? ReplicationTaskArn { get; set; }

    [CliOption("--replication-instance-arn")]
    public string? ReplicationInstanceArn { get; set; }

    [CliOption("--source-engine-name")]
    public string? SourceEngineName { get; set; }

    [CliOption("--target-engine-name")]
    public string? TargetEngineName { get; set; }

    [CliOption("--migration-type")]
    public string? MigrationType { get; set; }

    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}