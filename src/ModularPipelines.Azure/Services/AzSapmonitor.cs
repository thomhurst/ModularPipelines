using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSapmonitor
{
    public AzSapmonitor(
        AzSapmonitorProviderInstance providerInstance,
        ICommand internalCommand
    )
    {
        ProviderInstance = providerInstance;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSapmonitorProviderInstance ProviderInstance { get; }

    public async Task<CommandResult> Create(AzSapmonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSapmonitorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSapmonitorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSapmonitorListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSapmonitorShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSapmonitorUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}