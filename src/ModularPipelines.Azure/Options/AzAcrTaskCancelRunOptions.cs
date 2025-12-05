using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "cancel-run")]
public record AzAcrTaskCancelRunOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--run-id")] string RunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}