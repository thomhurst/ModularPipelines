using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "list-usage")]
public record AzVmListUsageOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;