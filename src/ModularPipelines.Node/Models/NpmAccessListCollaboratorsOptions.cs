using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("access", "list", "collaborators")]
public record NpmAccessListCollaboratorsOptions : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Package { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? User { get; set; }
}