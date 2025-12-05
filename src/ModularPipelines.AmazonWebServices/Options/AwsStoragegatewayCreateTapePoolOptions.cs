using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-tape-pool")]
public record AwsStoragegatewayCreateTapePoolOptions(
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--storage-class")] string StorageClass
) : AwsOptions
{
    [CliOption("--retention-lock-type")]
    public string? RetentionLockType { get; set; }

    [CliOption("--retention-lock-time-in-days")]
    public int? RetentionLockTimeInDays { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}