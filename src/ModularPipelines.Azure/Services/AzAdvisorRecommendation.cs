using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("advisor")]
public class AzAdvisorRecommendation
{
    public AzAdvisorRecommendation(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(AzAdvisorRecommendationDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdvisorRecommendationDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzAdvisorRecommendationEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdvisorRecommendationEnableOptions(), token);
    }

    public async Task<CommandResult> List(AzAdvisorRecommendationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdvisorRecommendationListOptions(), token);
    }
}