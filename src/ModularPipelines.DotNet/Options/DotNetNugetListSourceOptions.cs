using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "list", "source")]
[ExcludeFromCodeCoverage]
public record DotNetNugetListSourceOptions : DotNetOptions
{
    [BooleanCommandSwitch("--format")]
    public bool? Format { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }
}
