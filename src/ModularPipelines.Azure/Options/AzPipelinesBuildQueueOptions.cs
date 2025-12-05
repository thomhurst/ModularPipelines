using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "build", "queue")]
public record AzPipelinesBuildQueueOptions : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliOption("--definition-name")]
    public string? DefinitionName { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--queue-id")]
    public string? QueueId { get; set; }

    [CliOption("--variables")]
    public string? Variables { get; set; }
}