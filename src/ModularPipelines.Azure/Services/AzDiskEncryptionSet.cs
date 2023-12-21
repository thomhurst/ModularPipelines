using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDiskEncryptionSet
{
    public AzDiskEncryptionSet(
        AzDiskEncryptionSetIdentity identity,
        ICommand internalCommand
    )
    {
        Identity = identity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDiskEncryptionSetIdentity Identity { get; }

    public async Task<CommandResult> Create(AzDiskEncryptionSetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDiskEncryptionSetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDiskEncryptionSetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetListOptions(), token);
    }

    public async Task<CommandResult> ListAssociatedResources(AzDiskEncryptionSetListAssociatedResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDiskEncryptionSetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDiskEncryptionSetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDiskEncryptionSetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetWaitOptions(), token);
    }
}