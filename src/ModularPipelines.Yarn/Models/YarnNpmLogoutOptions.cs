using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "logout")]
public record YarnNpmLogoutOptions : YarnOptions
{
    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliFlag("--publish")]
    public virtual bool? Publish { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}