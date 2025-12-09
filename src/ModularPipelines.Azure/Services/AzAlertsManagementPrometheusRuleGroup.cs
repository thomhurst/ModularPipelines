using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("alerts-management")]
public class AzAlertsManagementPrometheusRuleGroup
{
    public AzAlertsManagementPrometheusRuleGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAlertsManagementPrometheusRuleGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAlertsManagementPrometheusRuleGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAlertsManagementPrometheusRuleGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAlertsManagementPrometheusRuleGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAlertsManagementPrometheusRuleGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAlertsManagementPrometheusRuleGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAlertsManagementPrometheusRuleGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAlertsManagementPrometheusRuleGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAlertsManagementPrometheusRuleGroupUpdateOptions(), token);
    }
}