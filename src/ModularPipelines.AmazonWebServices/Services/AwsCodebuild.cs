using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCodebuild
{
    public AwsCodebuild(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchDeleteBuilds(AwsCodebuildBatchDeleteBuildsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetBuildBatches(AwsCodebuildBatchGetBuildBatchesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetBuilds(AwsCodebuildBatchGetBuildsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetProjects(AwsCodebuildBatchGetProjectsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetReportGroups(AwsCodebuildBatchGetReportGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetReports(AwsCodebuildBatchGetReportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProject(AwsCodebuildCreateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReportGroup(AwsCodebuildCreateReportGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWebhook(AwsCodebuildCreateWebhookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBuildBatch(AwsCodebuildDeleteBuildBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProject(AwsCodebuildDeleteProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReport(AwsCodebuildDeleteReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReportGroup(AwsCodebuildDeleteReportGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsCodebuildDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSourceCredentials(AwsCodebuildDeleteSourceCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWebhook(AwsCodebuildDeleteWebhookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCodeCoverages(AwsCodebuildDescribeCodeCoveragesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTestCases(AwsCodebuildDescribeTestCasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReportGroupTrend(AwsCodebuildGetReportGroupTrendOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsCodebuildGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportSourceCredentials(AwsCodebuildImportSourceCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InvalidateProjectCache(AwsCodebuildInvalidateProjectCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBuildBatches(AwsCodebuildListBuildBatchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListBuildBatchesOptions(), token);
    }

    public async Task<CommandResult> ListBuildBatchesForProject(AwsCodebuildListBuildBatchesForProjectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListBuildBatchesForProjectOptions(), token);
    }

    public async Task<CommandResult> ListBuilds(AwsCodebuildListBuildsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListBuildsOptions(), token);
    }

    public async Task<CommandResult> ListBuildsForProject(AwsCodebuildListBuildsForProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCuratedEnvironmentImages(AwsCodebuildListCuratedEnvironmentImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListCuratedEnvironmentImagesOptions(), token);
    }

    public async Task<CommandResult> ListProjects(AwsCodebuildListProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListProjectsOptions(), token);
    }

    public async Task<CommandResult> ListReportGroups(AwsCodebuildListReportGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListReportGroupsOptions(), token);
    }

    public async Task<CommandResult> ListReports(AwsCodebuildListReportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListReportsOptions(), token);
    }

    public async Task<CommandResult> ListReportsForReportGroup(AwsCodebuildListReportsForReportGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSharedProjects(AwsCodebuildListSharedProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListSharedProjectsOptions(), token);
    }

    public async Task<CommandResult> ListSharedReportGroups(AwsCodebuildListSharedReportGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListSharedReportGroupsOptions(), token);
    }

    public async Task<CommandResult> ListSourceCredentials(AwsCodebuildListSourceCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildListSourceCredentialsOptions(), token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsCodebuildPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetryBuild(AwsCodebuildRetryBuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildRetryBuildOptions(), token);
    }

    public async Task<CommandResult> RetryBuildBatch(AwsCodebuildRetryBuildBatchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodebuildRetryBuildBatchOptions(), token);
    }

    public async Task<CommandResult> StartBuild(AwsCodebuildStartBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBuildBatch(AwsCodebuildStartBuildBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopBuild(AwsCodebuildStopBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopBuildBatch(AwsCodebuildStopBuildBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProject(AwsCodebuildUpdateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProjectVisibility(AwsCodebuildUpdateProjectVisibilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReportGroup(AwsCodebuildUpdateReportGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWebhook(AwsCodebuildUpdateWebhookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}