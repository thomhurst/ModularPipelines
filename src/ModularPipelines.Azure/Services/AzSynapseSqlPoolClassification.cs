using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql", "pool")]
public class AzSynapseSqlPoolClassification
{
    public AzSynapseSqlPoolClassification(
        AzSynapseSqlPoolClassificationRecommendation recommendation,
        ICommand internalCommand
    )
    {
        Recommendation = recommendation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSynapseSqlPoolClassificationRecommendation Recommendation { get; }

    public async Task<CommandResult> Create(AzSynapseSqlPoolClassificationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseSqlPoolClassificationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSynapseSqlPoolClassificationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseSqlPoolClassificationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSynapseSqlPoolClassificationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}