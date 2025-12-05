using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-storage")]
public record AwsKafkaUpdateStorageOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CliOption("--storage-mode")]
    public string? StorageMode { get; set; }

    [CliOption("--volume-size-gb")]
    public int? VolumeSizeGb { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}