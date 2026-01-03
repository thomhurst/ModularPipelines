using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Enable(AzSphereDeviceNetworkEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListFirewallRules(AzSphereDeviceNetworkListFirewallRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkListFirewallRulesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListInterfaces(AzSphereDeviceNetworkListInterfacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkListInterfacesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowDiagnostics(AzSphereDeviceNetworkShowDiagnosticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkShowDiagnosticsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowStatus(AzSphereDeviceNetworkShowStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceNetworkShowStatusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateInterface(AzSphereDeviceNetworkUpdateInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}