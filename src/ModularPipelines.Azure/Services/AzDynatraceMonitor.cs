using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynatrace")]
public class AzDynatraceMonitor
{
    public AzDynatraceMonitor(
        AzDynatraceMonitorSsoConfig ssoConfig,
        AzDynatraceMonitorTagRule tagRule,
        ICommand internalCommand
    )
    {
        SsoConfig = ssoConfig;
        TagRule = tagRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDynatraceMonitorSsoConfig SsoConfig { get; }

    public AzDynatraceMonitorTagRule TagRule { get; }

    public async Task<CommandResult> Create(AzDynatraceMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDynatraceMonitorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorDeleteOptions(), token);
    }

    public async Task<CommandResult> GetSsoDetail(AzDynatraceMonitorGetSsoDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVmHostPayload(AzDynatraceMonitorGetVmHostPayloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDynatraceMonitorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppService(AzDynatraceMonitorListAppServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHost(AzDynatraceMonitorListHostOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLinkableEnvironment(AzDynatraceMonitorListLinkableEnvironmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMonitoredResource(AzDynatraceMonitorListMonitoredResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDynatraceMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDynatraceMonitorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDynatraceMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorWaitOptions(), token);
    }
}