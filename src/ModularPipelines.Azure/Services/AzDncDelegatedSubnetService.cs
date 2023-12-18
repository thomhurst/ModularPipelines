using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dnc")]
public class AzDncDelegatedSubnetService
{
    public AzDncDelegatedSubnetService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDncDelegatedSubnetServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDncDelegatedSubnetServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDncDelegatedSubnetServiceDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzDncDelegatedSubnetServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDncDelegatedSubnetServiceShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDncDelegatedSubnetServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDncDelegatedSubnetServiceWaitOptions(), token);
    }
}