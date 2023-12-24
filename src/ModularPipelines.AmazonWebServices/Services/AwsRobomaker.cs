using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRobomaker
{
    public AwsRobomaker(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchDeleteWorlds(AwsRobomakerBatchDeleteWorldsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDescribeSimulationJob(AwsRobomakerBatchDescribeSimulationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelSimulationJob(AwsRobomakerCancelSimulationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelSimulationJobBatch(AwsRobomakerCancelSimulationJobBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelWorldExportJob(AwsRobomakerCancelWorldExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelWorldGenerationJob(AwsRobomakerCancelWorldGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRobotApplication(AwsRobomakerCreateRobotApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRobotApplicationVersion(AwsRobomakerCreateRobotApplicationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSimulationApplication(AwsRobomakerCreateSimulationApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSimulationApplicationVersion(AwsRobomakerCreateSimulationApplicationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSimulationJob(AwsRobomakerCreateSimulationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorldExportJob(AwsRobomakerCreateWorldExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorldGenerationJob(AwsRobomakerCreateWorldGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorldTemplate(AwsRobomakerCreateWorldTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerCreateWorldTemplateOptions(), token);
    }

    public async Task<CommandResult> DeleteRobotApplication(AwsRobomakerDeleteRobotApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSimulationApplication(AwsRobomakerDeleteSimulationApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorldTemplate(AwsRobomakerDeleteWorldTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRobotApplication(AwsRobomakerDescribeRobotApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSimulationApplication(AwsRobomakerDescribeSimulationApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSimulationJob(AwsRobomakerDescribeSimulationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSimulationJobBatch(AwsRobomakerDescribeSimulationJobBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorld(AwsRobomakerDescribeWorldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorldExportJob(AwsRobomakerDescribeWorldExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorldGenerationJob(AwsRobomakerDescribeWorldGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorldTemplate(AwsRobomakerDescribeWorldTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorldTemplateBody(AwsRobomakerGetWorldTemplateBodyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerGetWorldTemplateBodyOptions(), token);
    }

    public async Task<CommandResult> ListRobotApplications(AwsRobomakerListRobotApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListRobotApplicationsOptions(), token);
    }

    public async Task<CommandResult> ListSimulationApplications(AwsRobomakerListSimulationApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListSimulationApplicationsOptions(), token);
    }

    public async Task<CommandResult> ListSimulationJobBatches(AwsRobomakerListSimulationJobBatchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListSimulationJobBatchesOptions(), token);
    }

    public async Task<CommandResult> ListSimulationJobs(AwsRobomakerListSimulationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListSimulationJobsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRobomakerListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWorldExportJobs(AwsRobomakerListWorldExportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListWorldExportJobsOptions(), token);
    }

    public async Task<CommandResult> ListWorldGenerationJobs(AwsRobomakerListWorldGenerationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListWorldGenerationJobsOptions(), token);
    }

    public async Task<CommandResult> ListWorldTemplates(AwsRobomakerListWorldTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListWorldTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListWorlds(AwsRobomakerListWorldsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRobomakerListWorldsOptions(), token);
    }

    public async Task<CommandResult> RestartSimulationJob(AwsRobomakerRestartSimulationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSimulationJobBatch(AwsRobomakerStartSimulationJobBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRobomakerTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRobomakerUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRobotApplication(AwsRobomakerUpdateRobotApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSimulationApplication(AwsRobomakerUpdateSimulationApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorldTemplate(AwsRobomakerUpdateWorldTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}