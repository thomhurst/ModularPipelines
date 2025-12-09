using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad")]
public class AzAdApp
{
    public AzAdApp(
        AzAdAppCredential credential,
        AzAdAppFederatedCredential federatedCredential,
        AzAdAppOwner owner,
        AzAdAppPermission permission,
        ICommand internalCommand
    )
    {
        Credential = credential;
        FederatedCredential = federatedCredential;
        Owner = owner;
        Permission = permission;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAdAppCredential Credential { get; }

    public AzAdAppFederatedCredential FederatedCredential { get; }

    public AzAdAppOwner Owner { get; }

    public AzAdAppPermission Permission { get; }

    public async Task<CommandResult> Create(AzAdAppCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAdAppDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAdAppListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdAppListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAdAppShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAdAppUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}