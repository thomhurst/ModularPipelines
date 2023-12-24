using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsComputeOptimizer
{
    public AwsComputeOptimizer(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DeleteRecommendationPreferences(AwsComputeOptimizerDeleteRecommendationPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRecommendationExportJobs(AwsComputeOptimizerDescribeRecommendationExportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerDescribeRecommendationExportJobsOptions(), token);
    }

    public async Task<CommandResult> ExportAutoScalingGroupRecommendations(AwsComputeOptimizerExportAutoScalingGroupRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportEbsVolumeRecommendations(AwsComputeOptimizerExportEbsVolumeRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportEc2InstanceRecommendations(AwsComputeOptimizerExportEc2InstanceRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportEcsServiceRecommendations(AwsComputeOptimizerExportEcsServiceRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportLambdaFunctionRecommendations(AwsComputeOptimizerExportLambdaFunctionRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportLicenseRecommendations(AwsComputeOptimizerExportLicenseRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAutoScalingGroupRecommendations(AwsComputeOptimizerGetAutoScalingGroupRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetAutoScalingGroupRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetEbsVolumeRecommendations(AwsComputeOptimizerGetEbsVolumeRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEbsVolumeRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetEc2InstanceRecommendations(AwsComputeOptimizerGetEc2InstanceRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEc2InstanceRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetEc2RecommendationProjectedMetrics(AwsComputeOptimizerGetEc2RecommendationProjectedMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEcsServiceRecommendationProjectedMetrics(AwsComputeOptimizerGetEcsServiceRecommendationProjectedMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEcsServiceRecommendations(AwsComputeOptimizerGetEcsServiceRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEcsServiceRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetEffectiveRecommendationPreferences(AwsComputeOptimizerGetEffectiveRecommendationPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEnrollmentStatus(AwsComputeOptimizerGetEnrollmentStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEnrollmentStatusOptions(), token);
    }

    public async Task<CommandResult> GetEnrollmentStatusesForOrganization(AwsComputeOptimizerGetEnrollmentStatusesForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEnrollmentStatusesForOrganizationOptions(), token);
    }

    public async Task<CommandResult> GetLambdaFunctionRecommendations(AwsComputeOptimizerGetLambdaFunctionRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetLambdaFunctionRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetLicenseRecommendations(AwsComputeOptimizerGetLicenseRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetLicenseRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetRecommendationPreferences(AwsComputeOptimizerGetRecommendationPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecommendationSummaries(AwsComputeOptimizerGetRecommendationSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetRecommendationSummariesOptions(), token);
    }

    public async Task<CommandResult> PutRecommendationPreferences(AwsComputeOptimizerPutRecommendationPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEnrollmentStatus(AwsComputeOptimizerUpdateEnrollmentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}