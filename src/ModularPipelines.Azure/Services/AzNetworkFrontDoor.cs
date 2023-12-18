using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkFrontDoor
{
    public AzNetworkFrontDoor(
        AzNetworkFrontDoorBackendPool backendPool,
        AzNetworkFrontDoorFrontendEndpoint frontendEndpoint,
        AzNetworkFrontDoorLoadBalancing loadBalancing,
        AzNetworkFrontDoorProbe probe,
        AzNetworkFrontDoorRoutingRule routingRule,
        AzNetworkFrontDoorRulesEngine rulesEngine,
        AzNetworkFrontDoorWafPolicy wafPolicy,
        ICommand internalCommand
    )
    {
        BackendPool = backendPool;
        FrontendEndpoint = frontendEndpoint;
        LoadBalancing = loadBalancing;
        Probe = probe;
        RoutingRule = routingRule;
        RulesEngine = rulesEngine;
        WafPolicy = wafPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorBackendPool BackendPool { get; }

    public AzNetworkFrontDoorFrontendEndpoint FrontendEndpoint { get; }

    public AzNetworkFrontDoorLoadBalancing LoadBalancing { get; }

    public AzNetworkFrontDoorProbe Probe { get; }

    public AzNetworkFrontDoorRoutingRule RoutingRule { get; }

    public AzNetworkFrontDoorRulesEngine RulesEngine { get; }

    public AzNetworkFrontDoorWafPolicy WafPolicy { get; }

    public async Task<CommandResult> CheckCustomDomain(AzNetworkFrontDoorCheckCustomDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckNameAvailability(AzNetworkFrontDoorCheckNameAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkFrontDoorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFrontDoorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurgeEndpoint(AzNetworkFrontDoorPurgeEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFrontDoorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFrontDoorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFrontDoorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFrontDoorWaitOptions(), token);
    }
}