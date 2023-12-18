using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricAclDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricAclListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricAclShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricAclUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricAclWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricAclWaitOptions(), token);
    }
}