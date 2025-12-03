using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nfd", "delete")]
public record AzAosmNfdDeleteOptions(
[property: CliOption("--config-file")] string ConfigFile,
[property: CliOption("--definition-type")] string DefinitionType
) : AzOptions
{
    [CliFlag("--clean")]
    public bool? Clean { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}