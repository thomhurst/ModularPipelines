using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "waf", "policy")]
public class AzCdnWafPolicyManagedRuleSet
{
    public AzCdnWafPolicyManagedRuleSet(
        AzCdnWafPolicyManagedRuleSetRuleGroupOverride ruleGroupOverride,
        ICommand internalCommand
    )
    {
        RuleGroupOverride = ruleGroupOverride;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCdnWafPolicyManagedRuleSetRuleGroupOverride RuleGroupOverride { get; }

    public async Task<CommandResult> Add(AzCdnWafPolicyManagedRuleSetAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCdnWafPolicyManagedRuleSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailable(AzCdnWafPolicyManagedRuleSetListAvailableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzCdnWafPolicyManagedRuleSetRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCdnWafPolicyManagedRuleSetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}