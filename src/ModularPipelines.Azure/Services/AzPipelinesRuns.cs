using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines")]
public class AzPipelinesRuns
{
    public AzPipelinesRuns(
        AzPipelinesRunsArtifact artifact,
        AzPipelinesRunsTag tag,
        ICommand internalCommand
    )
    {
        Artifact = artifact;
        Tag = tag;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPipelinesRunsArtifact Artifact { get; }

    public AzPipelinesRunsTag Tag { get; }

    public async Task<CommandResult> List(AzPipelinesRunsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPipelinesRunsShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}