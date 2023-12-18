using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "share-rm", "create")]
public record AzStorageShareRmCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--access-tier")]
    public string? AccessTier { get; set; }

    [BooleanCommandSwitch("--enabled-protocols")]
    public bool? EnabledProtocols { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--quota")]
    public string? Quota { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--root-squash")]
    public string? RootSquash { get; set; }
}