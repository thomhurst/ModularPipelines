using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation")]
public class AzAutomationRunbook
{
    public AzAutomationRunbook(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAutomationRunbookCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAutomationRunbookDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAutomationRunbookListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Publish(AzAutomationRunbookPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookPublishOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ReplaceContent(AzAutomationRunbookReplaceContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RevertToPublished(AzAutomationRunbookRevertToPublishedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookRevertToPublishedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAutomationRunbookShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzAutomationRunbookStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAutomationRunbookUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzAutomationRunbookWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationRunbookWaitOptions(), cancellationToken: token);
    }
}