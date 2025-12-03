using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "config", "add")]
public record AzSqlMiArcConfigAddOptions(
[property: CliOption("--json-values")] string JsonValues,
[property: CliOption("--path")] string Path
) : AzOptions;