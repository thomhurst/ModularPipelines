using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "get-versions")]
public record AzAkshybridGetVersionsOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;