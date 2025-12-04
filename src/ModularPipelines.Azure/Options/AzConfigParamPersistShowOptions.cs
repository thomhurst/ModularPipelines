using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("config", "param-persist", "show")]
public record AzConfigParamPersistShowOptions : AzOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }
}