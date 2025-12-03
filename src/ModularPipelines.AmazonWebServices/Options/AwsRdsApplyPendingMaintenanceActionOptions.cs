using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "apply-pending-maintenance-action")]
public record AwsRdsApplyPendingMaintenanceActionOptions(
[property: CliOption("--resource-identifier")] string ResourceIdentifier,
[property: CliOption("--apply-action")] string ApplyAction,
[property: CliOption("--opt-in-type")] string OptInType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}