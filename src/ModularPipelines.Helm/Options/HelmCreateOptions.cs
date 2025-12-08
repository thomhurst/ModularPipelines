using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliSubCommand("create")]
[ExcludeFromCodeCoverage]
public record HelmCreateOptions : HelmOptions
{
    [CliOption("--starter")]
    public virtual string? Starter { get; set; }
}