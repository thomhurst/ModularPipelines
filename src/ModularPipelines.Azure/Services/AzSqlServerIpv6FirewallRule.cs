using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "server")]
public class AzSqlServerIpv6FirewallRule
{
    public AzSqlServerIpv6FirewallRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSqlServerIpv6FirewallRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlServerIpv6FirewallRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerIpv6FirewallRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlServerIpv6FirewallRuleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerIpv6FirewallRuleListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlServerIpv6FirewallRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerIpv6FirewallRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlServerIpv6FirewallRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerIpv6FirewallRuleUpdateOptions(), token);
    }
}