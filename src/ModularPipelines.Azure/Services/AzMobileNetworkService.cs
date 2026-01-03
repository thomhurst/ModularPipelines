using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network")]
public class AzMobileNetworkService
{
    public AzMobileNetworkService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMobileNetworkServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkServiceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMobileNetworkServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkServiceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkServiceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkServiceWaitOptions(), cancellationToken: token);
    }
}