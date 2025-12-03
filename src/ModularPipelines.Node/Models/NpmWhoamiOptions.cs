using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("whoami")]
public record NpmWhoamiOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }
}