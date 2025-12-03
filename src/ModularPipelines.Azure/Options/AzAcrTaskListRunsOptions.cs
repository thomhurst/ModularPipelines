using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "task", "list-runs")]
public record AzAcrTaskListRunsOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-status")]
    public string? RunStatus { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}