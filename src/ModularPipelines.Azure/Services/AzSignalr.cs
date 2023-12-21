using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSignalr
{
    public AzSignalr(
        AzSignalrCors cors,
        AzSignalrCustomCertificate customCertificate,
        AzSignalrCustomDomain customDomain,
        AzSignalrIdentity identity,
        AzSignalrKey key,
        AzSignalrNetworkRule networkRule,
        AzSignalrReplica replica,
        AzSignalrUpstream upstream,
        ICommand internalCommand
    )
    {
        Cors = cors;
        CustomCertificate = customCertificate;
        CustomDomain = customDomain;
        Identity = identity;
        Key = key;
        NetworkRule = networkRule;
        Replica = replica;
        Upstream = upstream;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSignalrCors Cors { get; }

    public AzSignalrCustomCertificate CustomCertificate { get; }

    public AzSignalrCustomDomain CustomDomain { get; }

    public AzSignalrIdentity Identity { get; }

    public AzSignalrKey Key { get; }

    public AzSignalrNetworkRule NetworkRule { get; }

    public AzSignalrReplica Replica { get; }

    public AzSignalrUpstream Upstream { get; }

    public async Task<CommandResult> Create(AzSignalrCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSignalrDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSignalrDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSignalrListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSignalrListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzSignalrRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSignalrRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzSignalrShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSignalrShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSignalrUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSignalrUpdateOptions(), token);
    }
}