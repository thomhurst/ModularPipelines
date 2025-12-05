using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("stack-hci-vm", "network")]
public class AzStackHciVmNetworkLnet
{
    public AzStackHciVmNetworkLnet(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStackHciVmNetworkLnetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStackHciVmNetworkLnetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmNetworkLnetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStackHciVmNetworkLnetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmNetworkLnetListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStackHciVmNetworkLnetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmNetworkLnetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciVmNetworkLnetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciVmNetworkLnetUpdateOptions(), token);
    }
}