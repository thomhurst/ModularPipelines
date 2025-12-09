using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "whoami")]
public record YarnNpmWhoamiOptions : YarnOptions
{
    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliFlag("--publish")]
    public virtual bool? Publish { get; set; }
}