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
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "remove";
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Package { get; }
}
