using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetAutocleanOptions : AptGetOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "autoclean";
}