using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("optimize")]
public record OptimizeOptions : ChocoOptions
{
    [BooleanCommandSwitch("--deflate-nupkg-only")]
    public bool? DeflateNupkgOnly { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}