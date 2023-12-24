using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "batch-delete-document")]
public record AwsKendraBatchDeleteDocumentOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--document-id-list")] string[] DocumentIdList
) : AwsOptions
{
    [CommandSwitch("--data-source-sync-job-metric-target")]
    public string? DataSourceSyncJobMetricTarget { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}