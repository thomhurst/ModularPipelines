using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "stage")]
public record AzIotDuUpdateStageOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--manifest-path")] string ManifestPath,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--storage-container")] string StorageContainer
) : AzOptions
{
    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-subscription")]
    public string? StorageSubscription { get; set; }

    [BooleanCommandSwitch("--then-import")]
    public bool? ThenImport { get; set; }
}