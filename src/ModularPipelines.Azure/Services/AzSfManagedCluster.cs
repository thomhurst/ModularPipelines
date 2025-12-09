using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf")]
public class AzSfManagedCluster
{
    public AzSfManagedCluster(
        AzSfManagedClusterClientCertificate clientCertificate,
        AzSfManagedClusterNetworkSecurityRule networkSecurityRule,
        ICommand internalCommand
    )
    {
        ClientCertificate = clientCertificate;
        NetworkSecurityRule = networkSecurityRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSfManagedClusterClientCertificate ClientCertificate { get; }

    public AzSfManagedClusterNetworkSecurityRule NetworkSecurityRule { get; }

    public async Task<CommandResult> Create(AzSfManagedClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSfManagedClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSfManagedClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSfManagedClusterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSfManagedClusterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSfManagedClusterUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}