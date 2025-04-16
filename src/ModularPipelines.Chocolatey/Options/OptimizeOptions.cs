using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("optimize")]
public record OptimizeOptions : ChocoOptions
{
    [BooleanCommandSwitch("--deflate-nupkg-only")]
    public virtual bool? DeflateNupkgOnly { get; set; }

    [CommandSwitch("--id")]
    public virtual string? Id { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}