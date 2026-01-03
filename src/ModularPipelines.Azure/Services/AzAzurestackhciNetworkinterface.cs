using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci")]
public class AzAzurestackhciNetworkinterface
{
    public AzAzurestackhciNetworkinterface(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciNetworkinterfaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciNetworkinterfaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciNetworkinterfaceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAzurestackhciNetworkinterfaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciNetworkinterfaceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciNetworkinterfaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciNetworkinterfaceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciNetworkinterfaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciNetworkinterfaceUpdateOptions(), cancellationToken: token);
    }
}