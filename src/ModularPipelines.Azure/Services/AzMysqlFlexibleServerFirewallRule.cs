using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server")]
public class AzMysqlFlexibleServerFirewallRule
{
    public AzMysqlFlexibleServerFirewallRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMysqlFlexibleServerFirewallRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMysqlFlexibleServerFirewallRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlFlexibleServerFirewallRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMysqlFlexibleServerFirewallRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMysqlFlexibleServerFirewallRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlFlexibleServerFirewallRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMysqlFlexibleServerFirewallRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlFlexibleServerFirewallRuleUpdateOptions(), token);
    }
}