using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin")]
public class AzDevcenterAdminAttachedNetwork
{
    public AzDevcenterAdminAttachedNetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminAttachedNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminAttachedNetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminAttachedNetworkDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminAttachedNetworkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminAttachedNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminAttachedNetworkShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminAttachedNetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminAttachedNetworkWaitOptions(), cancellationToken: token);
    }
}