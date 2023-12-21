using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway")]
public class AzNetworkApplicationGatewayWafPolicy
{
    public AzNetworkApplicationGatewayWafPolicy(
        AzNetworkApplicationGatewayWafPolicyCustomRule customRule,
        AzNetworkApplicationGatewayWafPolicyManagedRule managedRule,
        AzNetworkApplicationGatewayWafPolicyPolicySetting policySetting,
        ICommand internalCommand
    )
    {
        CustomRule = customRule;
        ManagedRule = managedRule;
        PolicySetting = policySetting;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayWafPolicyCustomRule CustomRule { get; }

    public AzNetworkApplicationGatewayWafPolicyManagedRule ManagedRule { get; }

    public AzNetworkApplicationGatewayWafPolicyPolicySetting PolicySetting { get; }

    public async Task<CommandResult> Create(AzNetworkApplicationGatewayWafPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkApplicationGatewayWafPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWafPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayWafPolicyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWafPolicyListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewayWafPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWafPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkApplicationGatewayWafPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWafPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkApplicationGatewayWafPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWafPolicyWaitOptions(), token);
    }
}