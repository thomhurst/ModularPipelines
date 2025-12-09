using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("hook", "update")]
public record NpmHookUpdateOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Id,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Url,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Secret
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}