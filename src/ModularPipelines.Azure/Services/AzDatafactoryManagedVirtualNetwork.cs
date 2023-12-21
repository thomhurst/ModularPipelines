using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
public class AzDatafactoryManagedVirtualNetwork
{
    public AzDatafactoryManagedVirtualNetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatafactoryManagedVirtualNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatafactoryManagedVirtualNetworkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatafactoryManagedVirtualNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryManagedVirtualNetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryManagedVirtualNetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryManagedVirtualNetworkUpdateOptions(), token);
    }
}