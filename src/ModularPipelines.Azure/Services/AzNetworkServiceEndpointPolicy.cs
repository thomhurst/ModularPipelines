using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "service-endpoint")]
public class AzNetworkServiceEndpointPolicy
{
    public AzNetworkServiceEndpointPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkServiceEndpointPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkServiceEndpointPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkServiceEndpointPolicyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkServiceEndpointPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkServiceEndpointPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkServiceEndpointPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyWaitOptions(), token);
    }
}