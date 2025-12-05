using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("profile", "enable-2fa")]
public record NpmProfileEnable2faOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}