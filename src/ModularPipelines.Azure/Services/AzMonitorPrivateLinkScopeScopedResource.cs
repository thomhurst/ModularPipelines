using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "private-link-scope")]
public class AzMonitorPrivateLinkScopeScopedResource
{
    public AzMonitorPrivateLinkScopeScopedResource(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorPrivateLinkScopeScopedResourceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorPrivateLinkScopeScopedResourceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMonitorPrivateLinkScopeScopedResourceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorPrivateLinkScopeScopedResourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeScopedResourceShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMonitorPrivateLinkScopeScopedResourceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeScopedResourceWaitOptions(), token);
    }
}