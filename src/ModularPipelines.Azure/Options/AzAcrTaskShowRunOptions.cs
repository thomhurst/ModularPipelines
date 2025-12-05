using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "show-run")]
public record AzAcrTaskShowRunOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--run-id")] string RunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}