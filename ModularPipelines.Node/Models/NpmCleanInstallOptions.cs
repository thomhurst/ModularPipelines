using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmCleanInstallOptions : CommandLineOptions
{
    [CommandLongSwitch( "install-strategy" )]
    public string? InstallStrategy { get; init; }

    [CommandLongSwitch( "omit" )]
    public IEnumerable<string>? Omit { get; init; }
}
