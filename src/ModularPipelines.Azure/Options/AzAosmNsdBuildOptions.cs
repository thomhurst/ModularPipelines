using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aosm", "nsd", "build")]
public record AzAosmNsdBuildOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}