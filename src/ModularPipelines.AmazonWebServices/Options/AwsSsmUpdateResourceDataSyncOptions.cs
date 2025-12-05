using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-resource-data-sync")]
public record AwsSsmUpdateResourceDataSyncOptions(
[property: CliOption("--sync-name")] string SyncName,
[property: CliOption("--sync-type")] string SyncType,
[property: CliOption("--sync-source")] string SyncSource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}