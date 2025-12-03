using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("doctor")]
public record NpmDoctorOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Ping { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Versions { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Environment { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Permissions { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Cache { get; set; }
}