using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDeploy
{
    public GcloudDeploy(
        GcloudDeployAutomationRuns automationRuns,
        GcloudDeployAutomations automations,
        GcloudDeployCustomTargetTypes customTargetTypes,
        GcloudDeployDeliveryPipelines deliveryPipelines,
        GcloudDeployJobRuns jobRuns,
        GcloudDeployReleases releases,
        GcloudDeployRollouts rollouts,
        GcloudDeployTargets targets,
        ICommand internalCommand
    )
    {
        AutomationRuns = automationRuns;
        Automations = automations;
        CustomTargetTypes = customTargetTypes;
        DeliveryPipelines = deliveryPipelines;
        JobRuns = jobRuns;
        Releases = releases;
        Rollouts = rollouts;
        Targets = targets;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDeployAutomationRuns AutomationRuns { get; }

    public GcloudDeployAutomations Automations { get; }

    public GcloudDeployCustomTargetTypes CustomTargetTypes { get; }

    public GcloudDeployDeliveryPipelines DeliveryPipelines { get; }

    public GcloudDeployJobRuns JobRuns { get; }

    public GcloudDeployReleases Releases { get; }

    public GcloudDeployRollouts Rollouts { get; }

    public GcloudDeployTargets Targets { get; }

    public async Task<CommandResult> Apply(GcloudDeployApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDeployDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfig(GcloudDeployGetConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDeployGetConfigOptions(), token);
    }
}