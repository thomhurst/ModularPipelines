using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm")]
public class AzScvmmCloud
{
    public AzScvmmCloud(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzScvmmCloudCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmCloudDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmCloudDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzScvmmCloudListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmCloudListOptions(), token);
    }

    public async Task<CommandResult> Show(AzScvmmCloudShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmCloudShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmCloudUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmCloudUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmCloudWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}