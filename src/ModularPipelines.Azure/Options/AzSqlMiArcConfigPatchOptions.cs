using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "config", "patch")]
public record AzSqlMiArcConfigPatchOptions(
[property: CommandSwitch("--patch-file")] string PatchFile,
[property: CommandSwitch("--path")] string Path
) : AzOptions;