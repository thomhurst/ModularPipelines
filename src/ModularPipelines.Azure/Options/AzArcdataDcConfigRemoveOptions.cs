using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "config", "remove")]
public record AzArcdataDcConfigRemoveOptions(
[property: CliOption("--json-path")] string JsonPath,
[property: CliOption("--path")] string Path
) : AzOptions;