using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "config", "replace")]
public record AzSqlMiArcConfigReplaceOptions(
[property: CommandSwitch("--json-values")] string JsonValues,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
}