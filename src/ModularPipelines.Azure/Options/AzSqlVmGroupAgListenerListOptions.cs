using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "vm", "group", "ag-listener", "list")]
public record AzSqlVmGroupAgListenerListOptions(
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;