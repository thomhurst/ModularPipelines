using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-tape-pool")]
public record AwsStoragegatewayCreateTapePoolOptions(
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--storage-class")] string StorageClass
) : AwsOptions
{
    [CommandSwitch("--retention-lock-type")]
    public string? RetentionLockType { get; set; }

    [CommandSwitch("--retention-lock-time-in-days")]
    public int? RetentionLockTimeInDays { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}