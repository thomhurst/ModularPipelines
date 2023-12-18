using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSigImageDefinitionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSigImageDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCommunity(AzSigImageDefinitionListCommunityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListShared(AzSigImageDefinitionListSharedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSigImageDefinitionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowCommunity(AzSigImageDefinitionShowCommunityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowShared(AzSigImageDefinitionShowSharedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSigImageDefinitionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzSigImageDefinitionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionWaitOptions(), token);
    }
}

