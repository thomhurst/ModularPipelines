using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-instance-storage-config")]
public record AwsConnectAssociateInstanceStorageConfigOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--storage-config")] string StorageConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}