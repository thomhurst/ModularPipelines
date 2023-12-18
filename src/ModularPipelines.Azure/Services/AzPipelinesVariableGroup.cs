using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines")]
public class AzPipelinesVariableGroup
{
    public AzPipelinesVariableGroup(
        AzPipelinesVariableGroupVariable variable,
        ICommand internalCommand
    )
    {
        Variable = variable;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPipelinesVariableGroupVariable Variable { get; }

    public async Task<CommandResult> Create(AzPipelinesVariableGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPipelinesVariableGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPipelinesVariableGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesVariableGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPipelinesVariableGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPipelinesVariableGroupUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}