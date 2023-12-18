using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "group", "ag-listener", "list")]
public record AzSqlVmGroupAgListenerListOptions(
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;