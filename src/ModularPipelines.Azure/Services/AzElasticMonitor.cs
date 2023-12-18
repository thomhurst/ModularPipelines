using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic")]
public class AzElasticMonitor
{
    public AzElasticMonitor(
        AzElasticMonitorTagRule tagRule,
        ICommand internalCommand
    )
    {
        TagRule = tagRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzElasticMonitorTagRule TagRule { get; }

    public async Task<CommandResult> AssociateTrafficFilter(AzElasticMonitorAssociateTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorAssociateTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> Create(AzElasticMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAndAssociateIpFilter(AzElasticMonitorCreateAndAssociateIpFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorCreateAndAssociateIpFilterOptions(), token);
    }

    public async Task<CommandResult> CreateAndAssociatePlFilter(AzElasticMonitorCreateAndAssociatePlFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorCreateAndAssociatePlFilterOptions(), token);
    }

    public async Task<CommandResult> CreateOrUpdateExternalUser(AzElasticMonitorCreateOrUpdateExternalUserOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorCreateOrUpdateExternalUserOptions(), token);
    }

    public async Task<CommandResult> Delete(AzElasticMonitorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorDeleteOptions(), token);
    }

    public async Task<CommandResult> DeleteTrafficFilter(AzElasticMonitorDeleteTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorDeleteTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> DetachAndDeleteTrafficFilter(AzElasticMonitorDetachAndDeleteTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorDetachAndDeleteTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> DetachTrafficFilter(AzElasticMonitorDetachTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorDetachTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> List(AzElasticMonitorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorListOptions(), token);
    }

    public async Task<CommandResult> ListAllTrafficFilter(AzElasticMonitorListAllTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorListAllTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> ListAssociatedTrafficFilter(AzElasticMonitorListAssociatedTrafficFilterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorListAssociatedTrafficFilterOptions(), token);
    }

    public async Task<CommandResult> ListDeploymentInfo(AzElasticMonitorListDeploymentInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorListDeploymentInfoOptions(), token);
    }

    public async Task<CommandResult> ListResource(AzElasticMonitorListResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUpgradableVersion(AzElasticMonitorListUpgradableVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorListUpgradableVersionOptions(), token);
    }

    public async Task<CommandResult> ListVmHost(AzElasticMonitorListVmHostOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzElasticMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzElasticMonitorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateVmCollection(AzElasticMonitorUpdateVmCollectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorUpdateVmCollectionOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzElasticMonitorUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorUpgradeOptions(), token);
    }

    public async Task<CommandResult> VmIngestionDetail(AzElasticMonitorVmIngestionDetailOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorVmIngestionDetailOptions(), token);
    }

    public async Task<CommandResult> Wait(AzElasticMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticMonitorWaitOptions(), token);
    }
}