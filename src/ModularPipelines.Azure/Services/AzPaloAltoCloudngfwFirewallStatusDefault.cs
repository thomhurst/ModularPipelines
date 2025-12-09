using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "firewall", "status")]
public class AzPaloAltoCloudngfwFirewallStatusDefault
{
    public AzPaloAltoCloudngfwFirewallStatusDefault(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwFirewallStatusDefaultShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwFirewallStatusDefaultShowOptions(), token);
    }
}