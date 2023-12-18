using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "append-persistent-storage")]
public record AzSpringCloudAppAppendPersistentStorageOptions(
[property: CommandSwitch("--mount-path")] string MountPath,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--persistent-storage-type")] string PersistentStorageType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--share-name")] string ShareName,
[property: CommandSwitch("--storage-name")] string StorageName
) : AzOptions
{
    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public bool? ReadOnly { get; set; }
}

