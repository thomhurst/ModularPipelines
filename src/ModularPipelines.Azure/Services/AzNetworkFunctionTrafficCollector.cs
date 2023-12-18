using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-function")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFunctionTrafficCollectorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFunctionTrafficCollectorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkFunctionTrafficCollectorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFunctionTrafficCollectorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFunctionTrafficCollectorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorWaitOptions(), token);
    }
}