using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("optimize")]
public record OptimizeOptions : ChocoOptions
{
    [CliFlag("--deflate-nupkg-only")]
    public virtual bool? DeflateNupkgOnly { get; set; }

    [CliOption("--id")]
    public virtual string? Id { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}