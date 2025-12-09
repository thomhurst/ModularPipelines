using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "update-cell")]
public record AwsRoute53RecoveryReadinessUpdateCellOptions(
[property: CliOption("--cell-name")] string CellName,
[property: CliOption("--cells")] string[] Cells
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}