using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "build", "queue")]
public record AzPipelinesBuildQueueOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--definition-id")]
    public string? DefinitionId { get; set; }

    [CommandSwitch("--definition-name")]
    public string? DefinitionName { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--open")]
    public bool? Open { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--queue-id")]
    public string? QueueId { get; set; }

    [CommandSwitch("--variables")]
    public string? Variables { get; set; }
}