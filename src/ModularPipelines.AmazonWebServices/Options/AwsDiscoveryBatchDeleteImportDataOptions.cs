using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "batch-delete-import-data")]
public record AwsDiscoveryBatchDeleteImportDataOptions(
[property: CommandSwitch("--import-task-ids")] string[] ImportTaskIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}