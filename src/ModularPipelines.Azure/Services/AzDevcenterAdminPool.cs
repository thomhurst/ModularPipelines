using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin")]
public class AzDevcenterAdminPool
{
    public AzDevcenterAdminPool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunHealthCheck(AzDevcenterAdminPoolRunHealthCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminPoolRunHealthCheckOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminPoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminPoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminPoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminPoolWaitOptions(), token);
    }
}