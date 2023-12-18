using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "append-persistent-storage")]
public record AzSpringAppAppendPersistentStorageOptions(
[property: CommandSwitch("--mount-path")] string MountPath,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--persistent-storage-type")] string PersistentStorageType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--storage-name")] string StorageName
) : AzOptions
{
    [BooleanCommandSwitch("--enable-sub-path")]
    public bool? EnableSubPath { get; set; }

    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public bool? ReadOnly { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }
}