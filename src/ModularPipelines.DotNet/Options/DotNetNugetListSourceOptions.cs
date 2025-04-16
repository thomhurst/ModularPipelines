using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "list", "source")]
[ExcludeFromCodeCoverage]
public record DotNetNugetListSourceOptions : DotNetOptions
{
    [BooleanCommandSwitch("--format")]
    public virtual bool? Format { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }
}
