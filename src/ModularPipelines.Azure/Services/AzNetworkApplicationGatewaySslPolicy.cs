using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway")]
public class AzNetworkApplicationGatewaySslPolicy
{
    public AzNetworkApplicationGatewaySslPolicy(
        AzNetworkApplicationGatewaySslPolicyPredefined predefined,
        ICommand internalCommand
    )
    {
        Predefined = predefined;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewaySslPolicyPredefined Predefined { get; }

    public async Task<CommandResult> ListOptions(AzNetworkApplicationGatewaySslPolicyListOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewaySslPolicyListOptionsOptions(), token);
    }

    public async Task<CommandResult> Set(AzNetworkApplicationGatewaySslPolicySetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewaySslPolicyShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkApplicationGatewaySslPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewaySslPolicyWaitOptions(), token);
    }
}