using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("logic")]
public class AzLogicWorkflow
{
    public AzLogicWorkflow(
        AzLogicWorkflowIdentity identity,
        ICommand internalCommand
    )
    {
        Identity = identity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLogicWorkflowIdentity Identity { get; }

    public async Task<CommandResult> Create(AzLogicWorkflowCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzLogicWorkflowDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicWorkflowDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzLogicWorkflowListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicWorkflowListOptions(), token);
    }

    public async Task<CommandResult> Show(AzLogicWorkflowShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicWorkflowShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzLogicWorkflowUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}