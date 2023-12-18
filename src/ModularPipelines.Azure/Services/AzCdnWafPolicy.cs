using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "waf")]
public class AzCdnWafPolicy
{
    public AzCdnWafPolicy(
        AzCdnWafPolicyCustomRule customRule,
        AzCdnWafPolicyManagedRuleSet managedRuleSet,
        AzCdnWafPolicyRateLimitRule rateLimitRule,
        ICommand internalCommand
    )
    {
        CustomRule = customRule;
        ManagedRuleSet = managedRuleSet;
        RateLimitRule = rateLimitRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCdnWafPolicyCustomRule CustomRule { get; }

    public AzCdnWafPolicyManagedRuleSet ManagedRuleSet { get; }

    public AzCdnWafPolicyRateLimitRule RateLimitRule { get; }

    public async Task<CommandResult> Delete(AzCdnWafPolicyDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCdnWafPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzCdnWafPolicySetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnWafPolicySetOptions(), token);
    }

    public async Task<CommandResult> Show(AzCdnWafPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnWafPolicyShowOptions(), token);
    }
}