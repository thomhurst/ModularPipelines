using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("disk-encryption-set")]
public class AzDiskEncryptionSetIdentity
{
    public AzDiskEncryptionSetIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Assign(AzDiskEncryptionSetIdentityAssignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetIdentityAssignOptions(), token);
    }

    public async Task<CommandResult> Remove(AzDiskEncryptionSetIdentityRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetIdentityRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzDiskEncryptionSetIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskEncryptionSetIdentityShowOptions(), token);
    }
}