using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("dependency", "list")]
[ExcludeFromCodeCoverage]
public record HelmDependencyListOptions : HelmOptions
{
    [CliOption("--max-col-width")]
    public virtual string? MaxColWidth { get; set; }
}