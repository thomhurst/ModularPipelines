using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("explain")]
public record NpmExplainOptions : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }
}