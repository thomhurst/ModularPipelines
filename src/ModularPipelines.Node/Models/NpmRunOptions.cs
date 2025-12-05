using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("run")]
public record NpmRunOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string ScriptName
) : NpmOptions;