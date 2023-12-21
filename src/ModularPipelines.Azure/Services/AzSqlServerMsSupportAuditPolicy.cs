using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "ms-support")]
public class AzSqlServerMsSupportAuditPolicy
{
    public AzSqlServerMsSupportAuditPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlServerMsSupportAuditPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerMsSupportAuditPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlServerMsSupportAuditPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerMsSupportAuditPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSqlServerMsSupportAuditPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerMsSupportAuditPolicyWaitOptions(), token);
    }
}