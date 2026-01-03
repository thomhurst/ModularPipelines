using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig")]
public class AzSigImageDefinition
{
    public AzSigImageDefinition(
        AzSigImageDefinitionListCommunity listCommunity,
        AzSigImageDefinitionShowCommunity showCommunity,
        ICommand internalCommand
    )
    {
        ListCommunityCommands = listCommunity;
        ShowCommunityCommands = showCommunity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSigImageDefinitionListCommunity ListCommunityCommands { get; }

    public AzSigImageDefinitionShowCommunity ShowCommunityCommands { get; }

    public async Task<CommandResult> Create(AzSigImageDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzSigImageDefinitionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSigImageDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListCommunity(AzSigImageDefinitionListCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionListCommunityOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListShared(AzSigImageDefinitionListSharedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionListSharedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSigImageDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowCommunity(AzSigImageDefinitionShowCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionShowCommunityOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowShared(AzSigImageDefinitionShowSharedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionShowSharedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSigImageDefinitionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSigImageDefinitionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionWaitOptions(), cancellationToken: token);
    }
}