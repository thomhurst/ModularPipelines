using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "list-node")]
public record AzDataboxedgeListNodeOptions(
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;