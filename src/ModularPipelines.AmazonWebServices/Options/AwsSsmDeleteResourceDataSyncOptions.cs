using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "delete-resource-data-sync")]
public record AwsSsmDeleteResourceDataSyncOptions(
[property: CliOption("--sync-name")] string SyncName
) : AwsOptions
{
    [CliOption("--sync-type")]
    public string? SyncType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}