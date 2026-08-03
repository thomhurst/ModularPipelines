using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("hook", "add")]
public record NpmHookAddOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Pkg,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Url,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Secret
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Type { get; set; }
}