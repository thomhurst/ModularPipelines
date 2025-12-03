using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("profile", "get")]
public record NpmProfileGetOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Key { get; set; }
}