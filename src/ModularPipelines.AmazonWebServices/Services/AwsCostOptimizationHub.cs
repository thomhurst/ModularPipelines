using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCostOptimizationHub
{
    public AwsCostOptimizationHub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetPreferences(AwsCostOptimizationHubGetPreferencesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubGetPreferencesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendation(AwsCostOptimizationHubGetRecommendationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEnrollmentStatuses(AwsCostOptimizationHubListEnrollmentStatusesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubListEnrollmentStatusesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecommendationSummaries(AwsCostOptimizationHubListRecommendationSummariesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecommendations(AwsCostOptimizationHubListRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubListRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEnrollmentStatus(AwsCostOptimizationHubUpdateEnrollmentStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePreferences(AwsCostOptimizationHubUpdatePreferencesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubUpdatePreferencesOptions(), executionOptions, cancellationToken);
    }
}