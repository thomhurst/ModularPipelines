using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetUpdateOptions : AptGetOptions
{
    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string CommandName { get; } = "update";
}
