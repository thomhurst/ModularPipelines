using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "apply-pending-maintenance-action")]
public record AwsRdsApplyPendingMaintenanceActionOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier,
[property: CommandSwitch("--apply-action")] string ApplyAction,
[property: CommandSwitch("--opt-in-type")] string OptInType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}