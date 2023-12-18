using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "config", "init")]
public record AzSqlMiArcConfigInitOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
}