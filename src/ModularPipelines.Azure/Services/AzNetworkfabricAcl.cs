using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
public class AzNetworkfabricAcl
{
    public AzNetworkfabricAcl(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricAclCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricAclDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkfabricAclListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricAclShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricAclUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricAclWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclWaitOptions(), cancellationToken: token);
    }
}