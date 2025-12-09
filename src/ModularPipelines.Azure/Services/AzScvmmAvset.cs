using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm")]
public class AzScvmmAvset
{
    public AzScvmmAvset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzScvmmAvsetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmAvsetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmAvsetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzScvmmAvsetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmAvsetListOptions(), token);
    }

    public async Task<CommandResult> Show(AzScvmmAvsetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmAvsetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmAvsetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmAvsetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmAvsetWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}