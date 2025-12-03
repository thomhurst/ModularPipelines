using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "batch-delete-import-data")]
public record AwsDiscoveryBatchDeleteImportDataOptions(
[property: CliOption("--import-task-ids")] string[] ImportTaskIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}