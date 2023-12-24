using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "put-storage-configuration")]
public record AwsIotsitewisePutStorageConfigurationOptions(
[property: CommandSwitch("--storage-type")] string StorageType
) : AwsOptions
{
    [CommandSwitch("--multi-layer-storage")]
    public string? MultiLayerStorage { get; set; }

    [CommandSwitch("--disassociated-data-storage")]
    public string? DisassociatedDataStorage { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--warm-tier")]
    public string? WarmTier { get; set; }

    [CommandSwitch("--warm-tier-retention-period")]
    public string? WarmTierRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}