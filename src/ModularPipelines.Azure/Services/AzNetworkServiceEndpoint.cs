using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkServiceEndpoint
{
    public AzNetworkServiceEndpoint(
        AzNetworkServiceEndpointPolicy policy,
        AzNetworkServiceEndpointPolicyDefinition policyDefinition,
        ICommand internalCommand
    )
    {
        Policy = policy;
        PolicyDefinition = policyDefinition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkServiceEndpointPolicy Policy { get; }

    public AzNetworkServiceEndpointPolicyDefinition PolicyDefinition { get; }

    public async Task<CommandResult> List(AzNetworkServiceEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}