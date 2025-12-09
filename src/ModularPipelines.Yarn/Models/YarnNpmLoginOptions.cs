using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "login")]
public record YarnNpmLoginOptions : YarnOptions
{
    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliFlag("--publish")]
    public virtual bool? Publish { get; set; }

    [CliFlag("--always-auth")]
    public virtual bool? AlwaysAuth { get; set; }
}