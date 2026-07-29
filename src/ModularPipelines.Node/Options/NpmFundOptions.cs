using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("fund")]
public record NpmFundOptions : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--browser")]
    public virtual string? Browser { get; set; }

    [CliFlag("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliOption("--which")]
    public virtual int? Which { get; set; }
}