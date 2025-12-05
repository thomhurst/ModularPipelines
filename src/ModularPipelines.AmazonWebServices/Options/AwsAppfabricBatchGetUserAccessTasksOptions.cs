using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "batch-get-user-access-tasks")]
public record AwsAppfabricBatchGetUserAccessTasksOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--task-id-list")] string[] TaskIdList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}