using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-instance-storage-config")]
public record AwsConnectUpdateInstanceStorageConfigOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--storage-config")] string StorageConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}