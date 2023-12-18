using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCognitiveservices
{
    public AzCognitiveservices(
        AzCognitiveservicesAccount account,
        AzCognitiveservicesCommitmentTier commitmentTier,
        AzCognitiveservicesModel model,
        AzCognitiveservicesUsage usage,
        ICommand internalCommand
    )
    {
        Account = account;
        CommitmentTier = commitmentTier;
        Model = model;
        Usage = usage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCognitiveservicesAccount Account { get; }

    public AzCognitiveservicesCommitmentTier CommitmentTier { get; }

    public AzCognitiveservicesModel Model { get; }

    public AzCognitiveservicesUsage Usage { get; }

    public async Task<CommandResult> List(AzCognitiveservicesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCognitiveservicesListOptions(), token);
    }
}