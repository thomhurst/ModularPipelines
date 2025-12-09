using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "pr", "work-item", "add")]
public record AzReposPrWorkItemAddOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--work-items")] string WorkItems
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}