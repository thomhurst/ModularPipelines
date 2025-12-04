using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "clone")]
public record AzMonitorCloneOptions(
[property: CliOption("--source-resource")] string SourceResource,
[property: CliOption("--target-resource")] string TargetResource
) : AzOptions
{
    [CliFlag("--always-clone")]
    public bool? AlwaysClone { get; set; }

    [CliOption("--types")]
    public string? Types { get; set; }
}