using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("hook", "update")]
public record NpmHookUpdateOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Id,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Url,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Secret
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}