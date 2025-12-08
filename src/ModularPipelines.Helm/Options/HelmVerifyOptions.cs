using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliSubCommand("verify")]
[ExcludeFromCodeCoverage]
public record HelmVerifyOptions : HelmOptions
{
    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }
}