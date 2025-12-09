using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines")]
public class AzPipelinesBuild
{
    public AzPipelinesBuild(
        AzPipelinesBuildDefinition definition,
        AzPipelinesBuildTag tag,
        ICommand internalCommand
    )
    {
        Definition = definition;
        Tag = tag;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPipelinesBuildDefinition Definition { get; }

    public AzPipelinesBuildTag Tag { get; }

    public async Task<CommandResult> Cancel(AzPipelinesBuildCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPipelinesBuildListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesBuildListOptions(), token);
    }

    public async Task<CommandResult> Queue(AzPipelinesBuildQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesBuildQueueOptions(), token);
    }

    public async Task<CommandResult> Show(AzPipelinesBuildShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}