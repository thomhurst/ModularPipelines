using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "alert", "list")]
public record AzDataboxedgeAlertListOptions(
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;