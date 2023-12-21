using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-policy")]
public class AzNetworkApplicationGatewaySslPolicyPredefined
{
    public AzNetworkApplicationGatewaySslPolicyPredefined(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzNetworkApplicationGatewaySslPolicyPredefinedListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewaySslPolicyPredefinedListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewaySslPolicyPredefinedShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}