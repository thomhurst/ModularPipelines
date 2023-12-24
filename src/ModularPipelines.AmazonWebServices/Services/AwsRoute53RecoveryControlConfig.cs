using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53RecoveryControlConfig
{
    public AwsRoute53RecoveryControlConfig(
        AwsRoute53RecoveryControlConfigWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsRoute53RecoveryControlConfigWait Wait { get; }

    public async Task<CommandResult> CreateCluster(AwsRoute53RecoveryControlConfigCreateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateControlPanel(AwsRoute53RecoveryControlConfigCreateControlPanelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRoutingControl(AwsRoute53RecoveryControlConfigCreateRoutingControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSafetyRule(AwsRoute53RecoveryControlConfigCreateSafetyRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryControlConfigCreateSafetyRuleOptions(), token);
    }

    public async Task<CommandResult> DeleteCluster(AwsRoute53RecoveryControlConfigDeleteClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteControlPanel(AwsRoute53RecoveryControlConfigDeleteControlPanelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRoutingControl(AwsRoute53RecoveryControlConfigDeleteRoutingControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSafetyRule(AwsRoute53RecoveryControlConfigDeleteSafetyRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCluster(AwsRoute53RecoveryControlConfigDescribeClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeControlPanel(AwsRoute53RecoveryControlConfigDescribeControlPanelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRoutingControl(AwsRoute53RecoveryControlConfigDescribeRoutingControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSafetyRule(AwsRoute53RecoveryControlConfigDescribeSafetyRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsRoute53RecoveryControlConfigGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociatedRoute53HealthChecks(AwsRoute53RecoveryControlConfigListAssociatedRoute53HealthChecksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusters(AwsRoute53RecoveryControlConfigListClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryControlConfigListClustersOptions(), token);
    }

    public async Task<CommandResult> ListControlPanels(AwsRoute53RecoveryControlConfigListControlPanelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryControlConfigListControlPanelsOptions(), token);
    }

    public async Task<CommandResult> ListRoutingControls(AwsRoute53RecoveryControlConfigListRoutingControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSafetyRules(AwsRoute53RecoveryControlConfigListSafetyRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRoute53RecoveryControlConfigListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRoute53RecoveryControlConfigTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRoute53RecoveryControlConfigUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateControlPanel(AwsRoute53RecoveryControlConfigUpdateControlPanelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRoutingControl(AwsRoute53RecoveryControlConfigUpdateRoutingControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSafetyRule(AwsRoute53RecoveryControlConfigUpdateSafetyRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryControlConfigUpdateSafetyRuleOptions(), token);
    }
}