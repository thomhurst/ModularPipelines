using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchAccount
{
    public AzBatchAccount(
        AzBatchAccountAutostorageKeys autostorageKeys,
        AzBatchAccountIdentity identity,
        AzBatchAccountKeys keys,
        AzBatchAccountNetworkProfile networkProfile,
        ICommand internalCommand
    )
    {
        AutostorageKeys = autostorageKeys;
        Identity = identity;
        Keys = keys;
        NetworkProfile = networkProfile;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchAccountAutostorageKeys AutostorageKeys { get; }

    public AzBatchAccountIdentity Identity { get; }

    public AzBatchAccountKeys Keys { get; }

    public AzBatchAccountNetworkProfile NetworkProfile { get; }

    public async Task<CommandResult> Create(AzBatchAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBatchAccountDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchAccountListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Login(AzBatchAccountLoginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OutboundEndpoints(AzBatchAccountOutboundEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzBatchAccountSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchAccountShowOptions(), token);
    }
}