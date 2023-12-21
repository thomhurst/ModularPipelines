using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "service-endpoint")]
public class AzNetworkServiceEndpointPolicyDefinition
{
    public AzNetworkServiceEndpointPolicyDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkServiceEndpointPolicyDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkServiceEndpointPolicyDefinitionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyDefinitionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkServiceEndpointPolicyDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkServiceEndpointPolicyDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyDefinitionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkServiceEndpointPolicyDefinitionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyDefinitionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkServiceEndpointPolicyDefinitionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkServiceEndpointPolicyDefinitionWaitOptions(), token);
    }
}