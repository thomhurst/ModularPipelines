using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("hook", "add")]
public record NpmHookAddOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Pkg,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Url,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Secret
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Type { get; set; }
}