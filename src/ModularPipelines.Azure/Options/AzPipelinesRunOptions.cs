using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "run")]
public record AzPipelinesRunOptions : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--folder-path")]
    public string? FolderPath { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--variables")]
    public string? Variables { get; set; }
}