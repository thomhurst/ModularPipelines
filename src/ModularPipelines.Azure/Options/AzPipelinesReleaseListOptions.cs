using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "release", "list")]
public record AzPipelinesReleaseListOptions : AzOptions
{
    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--source-branch")]
    public string? SourceBranch { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}