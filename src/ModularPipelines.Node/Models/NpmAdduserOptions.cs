using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("adduser")]
public record NpmAdduserOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--auth-type")]
    public virtual string? AuthType { get; set; }
}