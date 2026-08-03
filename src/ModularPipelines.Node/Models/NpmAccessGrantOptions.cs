using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("access", "grant")]
public record NpmAccessGrantOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Value,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Scope
) : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string? Package { get; set; }
}
