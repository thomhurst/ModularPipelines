using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack")]
public class AzPaloAltoCloudngfwLocalRulestackPrefixlist
{
    public AzPaloAltoCloudngfwLocalRulestackPrefixlist(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackPrefixlistCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackPrefixlistDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackPrefixlistDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackPrefixlistListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackPrefixlistShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackPrefixlistShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackPrefixlistWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackPrefixlistWaitOptions(), cancellationToken: token);
    }
}