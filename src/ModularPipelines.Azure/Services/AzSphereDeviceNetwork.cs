using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device")]
public class AzSphereDeviceNetwork
{
    public AzSphereDeviceNetwork(
        AzSphereDeviceNetworkProxy proxy,
        ICommand internalCommand
    )
    {
        Proxy = proxy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSphereDeviceNetworkProxy Proxy { get; }

    public async Task<CommandResult> Disable(AzSphereDeviceNetworkDisableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(AzSphereDeviceNetworkEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFirewallRules(AzSphereDeviceNetworkListFirewallRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkListFirewallRulesOptions(), token);
    }

    public async Task<CommandResult> ListInterfaces(AzSphereDeviceNetworkListInterfacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkListInterfacesOptions(), token);
    }

    public async Task<CommandResult> ShowDiagnostics(AzSphereDeviceNetworkShowDiagnosticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkShowDiagnosticsOptions(), token);
    }

    public async Task<CommandResult> ShowStatus(AzSphereDeviceNetworkShowStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkShowStatusOptions(), token);
    }

    public async Task<CommandResult> UpdateInterface(AzSphereDeviceNetworkUpdateInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}