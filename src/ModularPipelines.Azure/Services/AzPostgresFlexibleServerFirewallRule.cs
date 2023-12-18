using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server")]
public class AzPostgresFlexibleServerFirewallRule
{
    public AzPostgresFlexibleServerFirewallRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPostgresFlexibleServerFirewallRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPostgresFlexibleServerFirewallRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPostgresFlexibleServerFirewallRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPostgresFlexibleServerFirewallRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresFlexibleServerFirewallRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzPostgresFlexibleServerFirewallRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresFlexibleServerFirewallRuleUpdateOptions(), token);
    }
}