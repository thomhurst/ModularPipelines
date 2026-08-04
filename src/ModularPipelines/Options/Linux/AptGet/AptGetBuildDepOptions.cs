using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetBuildDepOptions : AptGetOptions
{
    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string CommandName { get; } = "build-dep";
}
