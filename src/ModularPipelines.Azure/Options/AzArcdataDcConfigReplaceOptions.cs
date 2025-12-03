using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "config", "replace")]
public record AzArcdataDcConfigReplaceOptions(
[property: CliOption("--json-values")] string JsonValues,
[property: CliOption("--path")] string Path
) : AzOptions;