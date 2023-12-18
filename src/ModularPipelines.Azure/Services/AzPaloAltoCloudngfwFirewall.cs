using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw")]
public class AzPaloAltoCloudngfwFirewall
{
    public AzPaloAltoCloudngfwFirewall(
        AzPaloAltoCloudngfwFirewallStatus status,
        ICommand internalCommand
    )
    {
        Status = status;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPaloAltoCloudngfwFirewallStatus Status { get; }

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwFirewallCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwFirewallDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwFirewallListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallListOptions(), token);
    }

    public async Task<CommandResult> SaveLogProfile(AzPaloAltoCloudngfwFirewallSaveLogProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallSaveLogProfileOptions(), token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwFirewallShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallShowOptions(), token);
    }

    public async Task<CommandResult> ShowLogProfile(AzPaloAltoCloudngfwFirewallShowLogProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallShowLogProfileOptions(), token);
    }

    public async Task<CommandResult> ShowSupportInfo(AzPaloAltoCloudngfwFirewallShowSupportInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallShowSupportInfoOptions(), token);
    }

    public async Task<CommandResult> Update(AzPaloAltoCloudngfwFirewallUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwFirewallWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallWaitOptions(), token);
    }
}