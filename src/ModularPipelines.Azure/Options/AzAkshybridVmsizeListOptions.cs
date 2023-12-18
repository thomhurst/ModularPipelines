using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "vmsize", "list")]
public record AzAkshybridVmsizeListOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;