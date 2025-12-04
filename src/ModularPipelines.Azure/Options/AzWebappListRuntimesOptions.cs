using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "list-runtimes")]
public record AzWebappListRuntimesOptions : AzOptions
{
    [CliOption("--os")]
    public string? Os { get; set; }
}