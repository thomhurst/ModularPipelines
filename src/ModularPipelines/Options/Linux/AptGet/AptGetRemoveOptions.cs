using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public partial record AptGetRemoveOptions : AptGetOptions
{
    public AptGetRemoveOptions(string package)
    {
        Package = package;
    }

    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string CommandName { get; } = "remove";

    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string Package { get; }
}