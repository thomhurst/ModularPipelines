using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "config", "add")]
public record AzSqlMiArcConfigAddOptions(
[property: CommandSwitch("--json-values")] string JsonValues,
[property: CommandSwitch("--path")] string Path
) : AzOptions;