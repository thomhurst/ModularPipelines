using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "config", "patch")]
public record AzArcdataDcConfigPatchOptions(
[property: CliOption("--patch-file")] string PatchFile,
[property: CliOption("--path")] string Path
) : AzOptions;