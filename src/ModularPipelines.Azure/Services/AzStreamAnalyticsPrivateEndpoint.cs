using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics")]
public class AzStreamAnalyticsPrivateEndpoint
{
    public AzStreamAnalyticsPrivateEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStreamAnalyticsPrivateEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStreamAnalyticsPrivateEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsPrivateEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStreamAnalyticsPrivateEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStreamAnalyticsPrivateEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsPrivateEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStreamAnalyticsPrivateEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsPrivateEndpointWaitOptions(), token);
    }
}