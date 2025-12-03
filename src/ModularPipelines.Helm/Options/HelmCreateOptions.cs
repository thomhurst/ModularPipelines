using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("create")]
[ExcludeFromCodeCoverage]
public record HelmCreateOptions : HelmOptions
{
    [CliOption("--starter")]
    public string? Starter { get; set; }
}