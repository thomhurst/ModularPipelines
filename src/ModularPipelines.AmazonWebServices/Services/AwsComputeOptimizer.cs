using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> DeleteRecommendationPreferences(AwsComputeOptimizerDeleteRecommendationPreferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRecommendationExportJobs(AwsComputeOptimizerDescribeRecommendationExportJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerDescribeRecommendationExportJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportAutoScalingGroupRecommendations(AwsComputeOptimizerExportAutoScalingGroupRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportEbsVolumeRecommendations(AwsComputeOptimizerExportEbsVolumeRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportEc2InstanceRecommendations(AwsComputeOptimizerExportEc2InstanceRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportEcsServiceRecommendations(AwsComputeOptimizerExportEcsServiceRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportLambdaFunctionRecommendations(AwsComputeOptimizerExportLambdaFunctionRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportLicenseRecommendations(AwsComputeOptimizerExportLicenseRecommendationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAutoScalingGroupRecommendations(AwsComputeOptimizerGetAutoScalingGroupRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetAutoScalingGroupRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEbsVolumeRecommendations(AwsComputeOptimizerGetEbsVolumeRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEbsVolumeRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEc2InstanceRecommendations(AwsComputeOptimizerGetEc2InstanceRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEc2InstanceRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEc2RecommendationProjectedMetrics(AwsComputeOptimizerGetEc2RecommendationProjectedMetricsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEcsServiceRecommendationProjectedMetrics(AwsComputeOptimizerGetEcsServiceRecommendationProjectedMetricsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEcsServiceRecommendations(AwsComputeOptimizerGetEcsServiceRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEcsServiceRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEffectiveRecommendationPreferences(AwsComputeOptimizerGetEffectiveRecommendationPreferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEnrollmentStatus(AwsComputeOptimizerGetEnrollmentStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEnrollmentStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEnrollmentStatusesForOrganization(AwsComputeOptimizerGetEnrollmentStatusesForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetEnrollmentStatusesForOrganizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLambdaFunctionRecommendations(AwsComputeOptimizerGetLambdaFunctionRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetLambdaFunctionRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLicenseRecommendations(AwsComputeOptimizerGetLicenseRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetLicenseRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendationPreferences(AwsComputeOptimizerGetRecommendationPreferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendationSummaries(AwsComputeOptimizerGetRecommendationSummariesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComputeOptimizerGetRecommendationSummariesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutRecommendationPreferences(AwsComputeOptimizerPutRecommendationPreferencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEnrollmentStatus(AwsComputeOptimizerUpdateEnrollmentStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}