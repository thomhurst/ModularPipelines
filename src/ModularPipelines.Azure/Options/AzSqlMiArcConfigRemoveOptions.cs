using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "config", "remove")]
public record AzSqlMiArcConfigRemoveOptions(
[property: CommandSwitch("--json-path")] string JsonPath,
[property: CommandSwitch("--path")] string Path
) : AzOptions;