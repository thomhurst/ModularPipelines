using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "config", "replace")]
public record AzSqlMiArcConfigReplaceOptions(
[property: CliOption("--json-values")] string JsonValues,
[property: CliOption("--path")] string Path
) : AzOptions;