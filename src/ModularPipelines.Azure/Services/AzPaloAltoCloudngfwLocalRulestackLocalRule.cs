using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack")]
public class AzPaloAltoCloudngfwLocalRulestackLocalRule
{
    public AzPaloAltoCloudngfwLocalRulestackLocalRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackLocalRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackLocalRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackLocalRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RefreshCounter(AzPaloAltoCloudngfwLocalRulestackLocalRuleRefreshCounterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackLocalRuleRefreshCounterOptions(), token);
    }

    public async Task<CommandResult> ResetCounter(AzPaloAltoCloudngfwLocalRulestackLocalRuleResetCounterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackLocalRuleResetCounterOptions(), token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackLocalRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackLocalRuleShowOptions(), token);
    }

    public async Task<CommandResult> ShowCounter(AzPaloAltoCloudngfwLocalRulestackLocalRuleShowCounterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackLocalRuleShowCounterOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackLocalRuleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackLocalRuleWaitOptions(), token);
    }
}