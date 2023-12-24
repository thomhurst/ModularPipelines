using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "create-cell")]
public record AwsRoute53RecoveryReadinessCreateCellOptions(
[property: CommandSwitch("--cell-name")] string CellName
) : AwsOptions
{
    [CommandSwitch("--cells")]
    public string[]? Cells { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}