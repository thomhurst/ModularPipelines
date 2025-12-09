using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetRemoveOptions : AptGetOptions
{
    public AptGetRemoveOptions(string package)
    {
        Package = package;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "remove";

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Package { get; }
}