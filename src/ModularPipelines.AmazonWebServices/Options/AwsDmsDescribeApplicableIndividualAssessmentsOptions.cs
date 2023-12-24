using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "describe-applicable-individual-assessments")]
public record AwsDmsDescribeApplicableIndividualAssessmentsOptions : AwsOptions
{
    [CommandSwitch("--replication-task-arn")]
    public string? ReplicationTaskArn { get; set; }

    [CommandSwitch("--replication-instance-arn")]
    public string? ReplicationInstanceArn { get; set; }

    [CommandSwitch("--source-engine-name")]
    public string? SourceEngineName { get; set; }

    [CommandSwitch("--target-engine-name")]
    public string? TargetEngineName { get; set; }

    [CommandSwitch("--migration-type")]
    public string? MigrationType { get; set; }

    [CommandSwitch("--max-records")]
    public int? MaxRecords { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}