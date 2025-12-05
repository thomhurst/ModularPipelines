using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "publish")]
public record YarnNpmPublishOptions : YarnOptions
{
    [CliOption("--access")]
    public virtual string? Access { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliFlag("--tolerate-republish")]
    public virtual bool? TolerateRepublish { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}