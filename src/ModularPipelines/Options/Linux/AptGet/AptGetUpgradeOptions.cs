using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public partial record AptGetUpgradeOptions : AptGetOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "upgrade";
}