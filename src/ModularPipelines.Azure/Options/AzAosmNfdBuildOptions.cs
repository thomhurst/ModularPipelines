using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aosm", "nfd", "build")]
public record AzAosmNfdBuildOptions(
[property: CliOption("--config-file")] string ConfigFile,
[property: CliOption("--definition-type")] string DefinitionType
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--interactive")]
    public bool? Interactive { get; set; }

    [CliFlag("--order-params")]
    public bool? OrderParams { get; set; }
}