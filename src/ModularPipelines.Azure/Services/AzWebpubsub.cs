using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzWebpubsub
{
    public AzWebpubsub(
        AzWebpubsubClient client,
        AzWebpubsubHub hub,
        AzWebpubsubKey key,
        AzWebpubsubNetworkRule networkRule,
        AzWebpubsubReplica replica,
        AzWebpubsubService service,
        ICommand internalCommand
    )
    {
        Client = client;
        Hub = hub;
        Key = key;
        NetworkRule = networkRule;
        Replica = replica;
        Service = service;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebpubsubClient Client { get; }

    public AzWebpubsubHub Hub { get; }

    public AzWebpubsubKey Key { get; }

    public AzWebpubsubNetworkRule NetworkRule { get; }

    public AzWebpubsubReplica Replica { get; }

    public AzWebpubsubService Service { get; }

    public async Task<CommandResult> Create(AzWebpubsubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzWebpubsubDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzWebpubsubListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzWebpubsubListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsage(AzWebpubsubListUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzWebpubsubRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebpubsubRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebpubsubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebpubsubShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzWebpubsubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebpubsubUpdateOptions(), token);
    }
}