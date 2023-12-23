using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsInspector
{
    public AwsInspector(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddAttributesToFindings(AwsInspectorAddAttributesToFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssessmentTarget(AwsInspectorCreateAssessmentTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssessmentTemplate(AwsInspectorCreateAssessmentTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateExclusionsPreview(AwsInspectorCreateExclusionsPreviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResourceGroup(AwsInspectorCreateResourceGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentRun(AwsInspectorDeleteAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentTarget(AwsInspectorDeleteAssessmentTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentTemplate(AwsInspectorDeleteAssessmentTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssessmentRuns(AwsInspectorDescribeAssessmentRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssessmentTargets(AwsInspectorDescribeAssessmentTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssessmentTemplates(AwsInspectorDescribeAssessmentTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCrossAccountAccessRole(AwsInspectorDescribeCrossAccountAccessRoleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorDescribeCrossAccountAccessRoleOptions(), token);
    }

    public async Task<CommandResult> DescribeExclusions(AwsInspectorDescribeExclusionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFindings(AwsInspectorDescribeFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResourceGroups(AwsInspectorDescribeResourceGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRulesPackages(AwsInspectorDescribeRulesPackagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssessmentReport(AwsInspectorGetAssessmentReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetExclusionsPreview(AwsInspectorGetExclusionsPreviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTelemetryMetadata(AwsInspectorGetTelemetryMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentRunAgents(AwsInspectorListAssessmentRunAgentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentRuns(AwsInspectorListAssessmentRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListAssessmentRunsOptions(), token);
    }

    public async Task<CommandResult> ListAssessmentTargets(AwsInspectorListAssessmentTargetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListAssessmentTargetsOptions(), token);
    }

    public async Task<CommandResult> ListAssessmentTemplates(AwsInspectorListAssessmentTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListAssessmentTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListEventSubscriptions(AwsInspectorListEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> ListExclusions(AwsInspectorListExclusionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFindings(AwsInspectorListFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListFindingsOptions(), token);
    }

    public async Task<CommandResult> ListRulesPackages(AwsInspectorListRulesPackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspectorListRulesPackagesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsInspectorListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PreviewAgents(AwsInspectorPreviewAgentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterCrossAccountAccessRole(AwsInspectorRegisterCrossAccountAccessRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveAttributesFromFindings(AwsInspectorRemoveAttributesFromFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTagsForResource(AwsInspectorSetTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAssessmentRun(AwsInspectorStartAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAssessmentRun(AwsInspectorStopAssessmentRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SubscribeToEvent(AwsInspectorSubscribeToEventOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnsubscribeFromEvent(AwsInspectorUnsubscribeFromEventOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentTarget(AwsInspectorUpdateAssessmentTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}