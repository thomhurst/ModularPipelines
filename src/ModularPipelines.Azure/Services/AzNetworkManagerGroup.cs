using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager")]
public class AzNetworkManagerGroup
{
    public AzNetworkManagerGroup(
        AzNetworkManagerGroupStaticMember staticMember,
        ICommand internalCommand
    )
    {
        StaticMember = staticMember;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkManagerGroupStaticMember StaticMember { get; }

    public async Task<CommandResult> Create(AzNetworkManagerGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkManagerGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkManagerGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerGroupWaitOptions(), token);
    }
}