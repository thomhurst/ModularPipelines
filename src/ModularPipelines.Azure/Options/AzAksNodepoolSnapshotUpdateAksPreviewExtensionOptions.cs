using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot", "update", "(aks-preview", "extension)")]
public record AzAksNodepoolSnapshotUpdateAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--tags")] string Tags
) : AzOptions;