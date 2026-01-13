using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public partial record AptGetBuildDepOptions : AptGetOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "build-dep";
}