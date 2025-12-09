using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi-arc", "config", "init")]
public record AzSqlMiArcConfigInitOptions(
[property: CliOption("--path")] string Path
) : AzOptions;