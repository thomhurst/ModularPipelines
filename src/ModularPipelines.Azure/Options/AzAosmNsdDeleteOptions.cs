using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nsd", "delete")]
public record AzAosmNsdDeleteOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AzOptions
{
    [CliFlag("--clean")]
    public bool? Clean { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}