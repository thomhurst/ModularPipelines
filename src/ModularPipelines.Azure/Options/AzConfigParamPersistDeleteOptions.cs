using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("config", "param-persist", "delete")]
public record AzConfigParamPersistDeleteOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliFlag("--purge")]
    public bool? Purge { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }
}