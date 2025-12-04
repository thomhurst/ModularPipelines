using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "append-persistent-storage")]
public record AzSpringAppAppendPersistentStorageOptions(
[property: CliOption("--mount-path")] string MountPath,
[property: CliOption("--name")] string Name,
[property: CliOption("--persistent-storage-type")] string PersistentStorageType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--storage-name")] string StorageName
) : AzOptions
{
    [CliFlag("--enable-sub-path")]
    public bool? EnableSubPath { get; set; }

    [CliOption("--mount-options")]
    public string? MountOptions { get; set; }

    [CliFlag("--read-only")]
    public bool? ReadOnly { get; set; }

    [CliOption("--share-name")]
    public string? ShareName { get; set; }
}