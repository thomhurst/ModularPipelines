using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> DescribeEntitiesDetectionV2Job(AwsComprehendmedicalDescribeEntitiesDetectionV2JobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeIcd10CmInferenceJob(AwsComprehendmedicalDescribeIcd10CmInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePhiDetectionJob(AwsComprehendmedicalDescribePhiDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRxNormInferenceJob(AwsComprehendmedicalDescribeRxNormInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSnomedctInferenceJob(AwsComprehendmedicalDescribeSnomedctInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectEntitiesV2(AwsComprehendmedicalDetectEntitiesV2Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectPhi(AwsComprehendmedicalDetectPhiOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InferIcd10Cm(AwsComprehendmedicalInferIcd10CmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InferRxNorm(AwsComprehendmedicalInferRxNormOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InferSnomedct(AwsComprehendmedicalInferSnomedctOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEntitiesDetectionV2Jobs(AwsComprehendmedicalListEntitiesDetectionV2JobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListEntitiesDetectionV2JobsOptions(), token);
    }

    public async Task<CommandResult> ListIcd10CmInferenceJobs(AwsComprehendmedicalListIcd10CmInferenceJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListIcd10CmInferenceJobsOptions(), token);
    }

    public async Task<CommandResult> ListPhiDetectionJobs(AwsComprehendmedicalListPhiDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListPhiDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListRxNormInferenceJobs(AwsComprehendmedicalListRxNormInferenceJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListRxNormInferenceJobsOptions(), token);
    }

    public async Task<CommandResult> ListSnomedctInferenceJobs(AwsComprehendmedicalListSnomedctInferenceJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendmedicalListSnomedctInferenceJobsOptions(), token);
    }

    public async Task<CommandResult> StartEntitiesDetectionV2Job(AwsComprehendmedicalStartEntitiesDetectionV2JobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartIcd10CmInferenceJob(AwsComprehendmedicalStartIcd10CmInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartPhiDetectionJob(AwsComprehendmedicalStartPhiDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRxNormInferenceJob(AwsComprehendmedicalStartRxNormInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSnomedctInferenceJob(AwsComprehendmedicalStartSnomedctInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEntitiesDetectionV2Job(AwsComprehendmedicalStopEntitiesDetectionV2JobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopIcd10CmInferenceJob(AwsComprehendmedicalStopIcd10CmInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopPhiDetectionJob(AwsComprehendmedicalStopPhiDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopRxNormInferenceJob(AwsComprehendmedicalStopRxNormInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSnomedctInferenceJob(AwsComprehendmedicalStopSnomedctInferenceJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}