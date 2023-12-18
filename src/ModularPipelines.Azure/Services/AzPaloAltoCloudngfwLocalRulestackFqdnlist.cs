using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack")]
public class AzPaloAltoCloudngfwLocalRulestackFqdnlist
{
    public AzPaloAltoCloudngfwLocalRulestackFqdnlist(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackFqdnlistCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackFqdnlistDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackFqdnlistDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackFqdnlistListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackFqdnlistShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackFqdnlistShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackFqdnlistWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackFqdnlistWaitOptions(), token);
    }
}