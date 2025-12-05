using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ad")]
public class AzAdSp
{
    public AzAdSp(
        AzAdSpCredential credential,
        AzAdSpOwner owner,
        ICommand internalCommand
    )
    {
        Credential = credential;
        Owner = owner;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAdSpCredential Credential { get; }

    public AzAdSpOwner Owner { get; }

    public async Task<CommandResult> Create(AzAdSpCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateForRbac(AzAdSpCreateForRbacOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdSpCreateForRbacOptions(), token);
    }

    public async Task<CommandResult> Delete(AzAdSpDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAdSpListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAdSpListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAdSpShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAdSpUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}