using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("star")]
public record NpmStarOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliFlag("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}