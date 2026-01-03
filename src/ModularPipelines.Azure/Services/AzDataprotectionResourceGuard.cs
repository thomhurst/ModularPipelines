using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionResourceGuardDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDataprotectionResourceGuardListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListProtectedOperations(AzDataprotectionResourceGuardListProtectedOperationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDataprotectionResourceGuardShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Unlock(AzDataprotectionResourceGuardUnlockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardUnlockOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDataprotectionResourceGuardUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionResourceGuardUpdateOptions(), cancellationToken: token);
    }
}