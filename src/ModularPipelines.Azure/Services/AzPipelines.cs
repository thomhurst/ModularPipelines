using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPipelines
{
    public AzPipelines(
        AzPipelinesAgent agent,
        AzPipelinesBuild build,
        AzPipelinesFolder folder,
        AzPipelinesPool pool,
        AzPipelinesQueue queue,
        AzPipelinesRelease release,
        AzPipelinesRuns runs,
        AzPipelinesVariable variable,
        AzPipelinesVariableGroup variableGroup,
        ICommand internalCommand
    )
    {
        Agent = agent;
        Build = build;
        Folder = folder;
        Pool = pool;
        Queue = queue;
        Release = release;
        Runs = runs;
        Variable = variable;
        VariableGroup = variableGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPipelinesAgent Agent { get; }

    public AzPipelinesBuild Build { get; }

    public AzPipelinesFolder Folder { get; }

    public AzPipelinesPool Pool { get; }

    public AzPipelinesQueue Queue { get; }

    public AzPipelinesRelease Release { get; }

    public AzPipelinesRuns Runs { get; }

    public AzPipelinesVariable Variable { get; }

    public AzPipelinesVariableGroup VariableGroup { get; }

    public async Task<CommandResult> Create(AzPipelinesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPipelinesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPipelinesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(AzPipelinesRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPipelinesShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPipelinesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}