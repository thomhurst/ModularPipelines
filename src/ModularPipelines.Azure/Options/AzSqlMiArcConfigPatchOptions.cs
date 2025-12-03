using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "config", "patch")]
public record AzSqlMiArcConfigPatchOptions(
[property: CliOption("--patch-file")] string PatchFile,
[property: CliOption("--path")] string Path
) : AzOptions;