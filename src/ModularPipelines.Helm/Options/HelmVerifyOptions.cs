using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("verify")]
[ExcludeFromCodeCoverage]
public record HelmVerifyOptions : HelmOptions
{
    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }
}