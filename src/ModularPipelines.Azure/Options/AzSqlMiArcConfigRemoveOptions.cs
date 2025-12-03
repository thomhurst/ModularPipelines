using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "config", "remove")]
public record AzSqlMiArcConfigRemoveOptions(
[property: CliOption("--json-path")] string JsonPath,
[property: CliOption("--path")] string Path
) : AzOptions;