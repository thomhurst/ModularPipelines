using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("owner", "add")]
public record NpmOwnerAddOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string User
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }
}
