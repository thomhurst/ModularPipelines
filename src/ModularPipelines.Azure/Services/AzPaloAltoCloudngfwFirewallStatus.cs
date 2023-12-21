using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "firewall")]
public class AzPaloAltoCloudngfwFirewallStatus
{
    public AzPaloAltoCloudngfwFirewallStatus(
        AzPaloAltoCloudngfwFirewallStatusDefault @default,
        ICommand internalCommand
    )
    {
        Default = @default;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPaloAltoCloudngfwFirewallStatusDefault Default { get; }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwFirewallStatusListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}