using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> ListFirewallRules(AzSphereDeviceNetworkListFirewallRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInterfaces(AzSphereDeviceNetworkListInterfacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDiagnostics(AzSphereDeviceNetworkShowDiagnosticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowStatus(AzSphereDeviceNetworkShowStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInterface(AzSphereDeviceNetworkUpdateInterfaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}