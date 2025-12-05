using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud")]
public class AzNetworkcloudClustermanager
{
    public AzNetworkcloudClustermanager(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudClustermanagerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudClustermanagerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClustermanagerDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudClustermanagerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClustermanagerListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudClustermanagerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClustermanagerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudClustermanagerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClustermanagerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudClustermanagerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClustermanagerWaitOptions(), token);
    }
}