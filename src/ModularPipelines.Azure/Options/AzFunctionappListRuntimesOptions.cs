using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "list-runtimes")]
public record AzFunctionappListRuntimesOptions : AzOptions
{
    [CliOption("--os")]
    public string? Os { get; set; }
}