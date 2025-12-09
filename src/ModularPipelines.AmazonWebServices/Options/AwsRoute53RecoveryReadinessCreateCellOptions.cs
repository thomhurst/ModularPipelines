using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "create-cell")]
public record AwsRoute53RecoveryReadinessCreateCellOptions(
[property: CliOption("--cell-name")] string CellName
) : AwsOptions
{
    [CliOption("--cells")]
    public string[]? Cells { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}