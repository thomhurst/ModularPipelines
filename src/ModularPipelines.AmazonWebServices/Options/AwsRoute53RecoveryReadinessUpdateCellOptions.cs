using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "update-cell")]
public record AwsRoute53RecoveryReadinessUpdateCellOptions(
[property: CommandSwitch("--cell-name")] string CellName,
[property: CommandSwitch("--cells")] string[] Cells
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}