using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "batch-delete-document")]
public record AwsQbusinessBatchDeleteDocumentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--documents")] string[] Documents,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--data-source-sync-id")]
    public string? DataSourceSyncId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}