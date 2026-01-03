using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-function")]
public class AzNetworkFunctionTrafficCollector
{
    public AzNetworkFunctionTrafficCollector(
        AzNetworkFunctionTrafficCollectorCollectorPolicy collectorPolicy,
        ICommand internalCommand
    )
    {
        CollectorPolicy = collectorPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFunctionTrafficCollectorCollectorPolicy CollectorPolicy { get; }

    public async Task<CommandResult> Create(AzNetworkFunctionTrafficCollectorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkFunctionTrafficCollectorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkFunctionTrafficCollectorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkFunctionTrafficCollectorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkFunctionTrafficCollectorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkFunctionTrafficCollectorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorWaitOptions(), cancellationToken: token);
    }
}