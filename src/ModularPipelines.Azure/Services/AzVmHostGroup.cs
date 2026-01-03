using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "host")]
public class AzVmHostGroup
{
    public AzVmHostGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmHostGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzVmHostGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGroupDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetInstanceView(AzVmHostGroupGetInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGroupGetInstanceViewOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzVmHostGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGroupListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzVmHostGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGroupShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzVmHostGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGroupUpdateOptions(), cancellationToken: token);
    }
}