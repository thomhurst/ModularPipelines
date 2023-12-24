using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-storage")]
public record AwsKafkaUpdateStorageOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CommandSwitch("--storage-mode")]
    public string? StorageMode { get; set; }

    [CommandSwitch("--volume-size-gb")]
    public int? VolumeSizeGb { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}