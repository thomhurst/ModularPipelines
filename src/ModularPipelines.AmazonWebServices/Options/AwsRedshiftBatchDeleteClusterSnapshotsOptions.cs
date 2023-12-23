using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "batch-delete-cluster-snapshots")]
public record AwsRedshiftBatchDeleteClusterSnapshotsOptions(
[property: CommandSwitch("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}