using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsComprehendmedical
{
    public AwsComprehendmedical(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DescribeEntitiesDetectionV2Job(AwsComprehendmedicalDescribeEntitiesDetectionV2JobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeIcd10CmInferenceJob(AwsComprehendmedicalDescribeIcd10CmInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePhiDetectionJob(AwsComprehendmedicalDescribePhiDetectionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRxNormInferenceJob(AwsComprehendmedicalDescribeRxNormInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnomedctInferenceJob(AwsComprehendmedicalDescribeSnomedctInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetectEntitiesV2(AwsComprehendmedicalDetectEntitiesV2Options options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DetectPhi(AwsComprehendmedicalDetectPhiOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InferIcd10Cm(AwsComprehendmedicalInferIcd10CmOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InferRxNorm(AwsComprehendmedicalInferRxNormOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InferSnomedct(AwsComprehendmedicalInferSnomedctOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEntitiesDetectionV2Jobs(AwsComprehendmedicalListEntitiesDetectionV2JobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListEntitiesDetectionV2JobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListIcd10CmInferenceJobs(AwsComprehendmedicalListIcd10CmInferenceJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListIcd10CmInferenceJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPhiDetectionJobs(AwsComprehendmedicalListPhiDetectionJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListPhiDetectionJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRxNormInferenceJobs(AwsComprehendmedicalListRxNormInferenceJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListRxNormInferenceJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSnomedctInferenceJobs(AwsComprehendmedicalListSnomedctInferenceJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListSnomedctInferenceJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartEntitiesDetectionV2Job(AwsComprehendmedicalStartEntitiesDetectionV2JobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartIcd10CmInferenceJob(AwsComprehendmedicalStartIcd10CmInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartPhiDetectionJob(AwsComprehendmedicalStartPhiDetectionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartRxNormInferenceJob(AwsComprehendmedicalStartRxNormInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartSnomedctInferenceJob(AwsComprehendmedicalStartSnomedctInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopEntitiesDetectionV2Job(AwsComprehendmedicalStopEntitiesDetectionV2JobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopIcd10CmInferenceJob(AwsComprehendmedicalStopIcd10CmInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopPhiDetectionJob(AwsComprehendmedicalStopPhiDetectionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopRxNormInferenceJob(AwsComprehendmedicalStopRxNormInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopSnomedctInferenceJob(AwsComprehendmedicalStopSnomedctInferenceJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}