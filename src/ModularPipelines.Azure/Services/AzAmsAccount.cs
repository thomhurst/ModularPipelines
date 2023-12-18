using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams")]
public class AzAmsAccount
{
    public AzAmsAccount(
        AzAmsAccountEncryption encryption,
        AzAmsAccountIdentity identity,
        AzAmsAccountMru mru,
        AzAmsAccountSp sp,
        AzAmsAccountStorage storage,
        ICommand internalCommand
    )
    {
        Encryption = encryption;
        Identity = identity;
        Mru = mru;
        Sp = sp;
        Storage = storage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAmsAccountEncryption Encryption { get; }

    public AzAmsAccountIdentity Identity { get; }

    public AzAmsAccountMru Mru { get; }

    public AzAmsAccountSp Sp { get; }

    public AzAmsAccountStorage Storage { get; }

    public async Task<CommandResult> CheckName(AzAmsAccountCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzAmsAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAmsAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAmsAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmsAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsAccountUpdateOptions(), token);
    }
}