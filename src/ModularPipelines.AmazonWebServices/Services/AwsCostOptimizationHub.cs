using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> GetPreferences(AwsCostOptimizationHubGetPreferencesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubGetPreferencesOptions(), token);
    }

    public async Task<CommandResult> GetRecommendation(AwsCostOptimizationHubGetRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEnrollmentStatuses(AwsCostOptimizationHubListEnrollmentStatusesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubListEnrollmentStatusesOptions(), token);
    }

    public async Task<CommandResult> ListRecommendationSummaries(AwsCostOptimizationHubListRecommendationSummariesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecommendations(AwsCostOptimizationHubListRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubListRecommendationsOptions(), token);
    }

    public async Task<CommandResult> UpdateEnrollmentStatus(AwsCostOptimizationHubUpdateEnrollmentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePreferences(AwsCostOptimizationHubUpdatePreferencesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCostOptimizationHubUpdatePreferencesOptions(), token);
    }
}