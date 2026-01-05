using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> GetActionRecommendations(AwsPersonalizeRuntimeGetActionRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeRuntimeGetActionRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPersonalizedRanking(AwsPersonalizeRuntimeGetPersonalizedRankingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendations(AwsPersonalizeRuntimeGetRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPersonalizeRuntimeGetRecommendationsOptions(), executionOptions, cancellationToken);
    }
}