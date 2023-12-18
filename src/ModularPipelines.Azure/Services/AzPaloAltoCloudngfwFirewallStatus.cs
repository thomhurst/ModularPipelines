using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "firewall")]
public class AzPaloAltoCloudngfwFirewallStatus
{
    public AzPaloAltoCloudngfwFirewallStatus(
        AzPaloAltoCloudngfwFirewallStatusDefault @Default,
        ICommand internalCommand
    )
    {
        Default = @Default;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPaloAltoCloudngfwFirewallStatusDefault Default { get; }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwFirewallStatusListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

