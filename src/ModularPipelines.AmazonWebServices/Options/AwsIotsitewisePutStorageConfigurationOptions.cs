using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "put-storage-configuration")]
public record AwsIotsitewisePutStorageConfigurationOptions(
[property: CliOption("--storage-type")] string StorageType
) : AwsOptions
{
    [CliOption("--multi-layer-storage")]
    public string? MultiLayerStorage { get; set; }

    [CliOption("--disassociated-data-storage")]
    public string? DisassociatedDataStorage { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--warm-tier")]
    public string? WarmTier { get; set; }

    [CliOption("--warm-tier-retention-period")]
    public string? WarmTierRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}