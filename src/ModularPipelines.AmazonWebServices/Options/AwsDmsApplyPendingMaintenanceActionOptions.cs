using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "apply-pending-maintenance-action")]
public record AwsDmsApplyPendingMaintenanceActionOptions(
[property: CliOption("--replication-instance-arn")] string ReplicationInstanceArn,
[property: CliOption("--apply-action")] string ApplyAction,
[property: CliOption("--opt-in-type")] string OptInType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}