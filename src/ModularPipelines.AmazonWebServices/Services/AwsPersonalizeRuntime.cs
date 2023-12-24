using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPersonalizeRuntime
{
    public AwsPersonalizeRuntime(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetActionRecommendations(AwsPersonalizeRuntimeGetActionRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeRuntimeGetActionRecommendationsOptions(), token);
    }

    public async Task<CommandResult> GetPersonalizedRanking(AwsPersonalizeRuntimeGetPersonalizedRankingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecommendations(AwsPersonalizeRuntimeGetRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeRuntimeGetRecommendationsOptions(), token);
    }
}