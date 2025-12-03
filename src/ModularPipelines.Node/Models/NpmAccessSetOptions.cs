using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("access", "set")]
public record NpmAccessSetOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Package { get; set; }
}