using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("doctor")]
public record NpmDoctorOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Ping { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Versions { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Environment { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Permissions { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Cache { get; set; }
}