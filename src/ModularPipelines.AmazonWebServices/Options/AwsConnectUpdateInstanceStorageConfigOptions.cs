using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-instance-storage-config")]
public record AwsConnectUpdateInstanceStorageConfigOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--storage-config")] string StorageConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}