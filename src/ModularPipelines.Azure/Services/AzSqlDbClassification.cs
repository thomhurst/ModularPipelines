using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db")]
public class AzSqlDbClassification
{
    public AzSqlDbClassification(
        AzSqlDbClassificationRecommendation recommendation,
        ICommand internalCommand
    )
    {
        Recommendation = recommendation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlDbClassificationRecommendation Recommendation { get; }

    public async Task<CommandResult> Delete(AzSqlDbClassificationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSqlDbClassificationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbClassificationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlDbClassificationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSqlDbClassificationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}