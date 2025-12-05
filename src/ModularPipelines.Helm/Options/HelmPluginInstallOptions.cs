using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("plugin", "install")]
[ExcludeFromCodeCoverage]
public record HelmPluginInstallOptions : HelmOptions
{
    [CliOption("--version")]
    public virtual string? Version { get; set; }
}