using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks")]
public class AzHdinsightOnAksClusterpool
{
    public AzHdinsightOnAksClusterpool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHdinsightOnAksClusterpoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHdinsightOnAksClusterpoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterpoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzHdinsightOnAksClusterpoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterpoolListOptions(), token);
    }

    public async Task<CommandResult> Show(AzHdinsightOnAksClusterpoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterpoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzHdinsightOnAksClusterpoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterpoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzHdinsightOnAksClusterpoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterpoolWaitOptions(), token);
    }
}