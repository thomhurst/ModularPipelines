using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "get-versions", "(aks-preview", "extension)")]
public record AzAksGetVersionsAksPreviewExtensionOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;