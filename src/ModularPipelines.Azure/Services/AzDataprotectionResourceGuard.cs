using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection")]
public class AzDataprotectionResourceGuard
{
    public AzDataprotectionResourceGuard(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDataprotectionResourceGuardCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionResourceGuardDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDataprotectionResourceGuardListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProtectedOperations(AzDataprotectionResourceGuardListProtectedOperationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDataprotectionResourceGuardShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardShowOptions(), token);
    }

    public async Task<CommandResult> Unlock(AzDataprotectionResourceGuardUnlockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardUnlockOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataprotectionResourceGuardUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardUpdateOptions(), token);
    }
}