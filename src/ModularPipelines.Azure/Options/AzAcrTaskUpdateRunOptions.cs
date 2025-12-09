using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "update-run")]
public record AzAcrTaskUpdateRunOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--run-id")] string RunId
) : AzOptions
{
    [CliFlag("--no-archive")]
    public bool? NoArchive { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}