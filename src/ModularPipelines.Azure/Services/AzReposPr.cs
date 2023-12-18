using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos")]
public class AzReposPr
{
    public AzReposPr(
        AzReposPrPolicy policy,
        AzReposPrReviewer reviewer,
        AzReposPrWorkItem workItem,
        ICommand internalCommand
    )
    {
        Policy = policy;
        Reviewer = reviewer;
        WorkItem = workItem;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzReposPrPolicy Policy { get; }

    public AzReposPrReviewer Reviewer { get; }

    public AzReposPrWorkItem WorkItem { get; }

    public async Task<CommandResult> Checkout(AzReposPrCheckoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzReposPrCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzReposPrListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetVote(AzReposPrSetVoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzReposPrShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzReposPrUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}