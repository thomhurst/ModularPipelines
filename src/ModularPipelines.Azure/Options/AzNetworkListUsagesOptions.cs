using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "list-usages")]
public record AzNetworkListUsagesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;