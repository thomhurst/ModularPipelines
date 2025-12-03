using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("config", "set", "registry")]
public record NpmConfigSetRegistryOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Registry
) : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--editor")]
    public virtual string? Editor { get; set; }

    [CliOption("--location")]
    public virtual string? Location { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Key { get; set; }
}