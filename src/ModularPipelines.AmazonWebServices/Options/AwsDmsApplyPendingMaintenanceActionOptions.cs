using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "apply-pending-maintenance-action")]
public record AwsDmsApplyPendingMaintenanceActionOptions(
[property: CommandSwitch("--replication-instance-arn")] string ReplicationInstanceArn,
[property: CommandSwitch("--apply-action")] string ApplyAction,
[property: CommandSwitch("--opt-in-type")] string OptInType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}