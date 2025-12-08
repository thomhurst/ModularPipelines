using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig")]
public class AzSigImageVersion
{
    public AzSigImageVersion(
        AzSigImageVersionListCommunity listCommunity,
        AzSigImageVersionShowCommunity showCommunity,
        ICommand internalCommand
    )
    {
        ListCommunityCommands = listCommunity;
        ShowCommunityCommands = showCommunity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSigImageVersionListCommunity ListCommunityCommands { get; }

    public AzSigImageVersionShowCommunity ShowCommunityCommands { get; }

    public async Task<CommandResult> Create(AzSigImageVersionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSigImageVersionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSigImageVersionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCommunity(AzSigImageVersionListCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionListCommunityOptions(), token);
    }

    public async Task<CommandResult> ListShared(AzSigImageVersionListSharedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionListSharedOptions(), token);
    }

    public async Task<CommandResult> Show(AzSigImageVersionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowCommunity(AzSigImageVersionShowCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionShowCommunityOptions(), token);
    }

    public async Task<CommandResult> ShowShared(AzSigImageVersionShowSharedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionShowSharedOptions(), token);
    }

    public async Task<CommandResult> Undelete(AzSigImageVersionUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSigImageVersionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzSigImageVersionWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}