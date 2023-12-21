using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres")]
public class AzPostgresServerArc
{
    public AzPostgresServerArc(
        AzPostgresServerArcEndpoint endpoint,
        ICommand internalCommand
    )
    {
        Endpoint = endpoint;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPostgresServerArcEndpoint Endpoint { get; }

    public async Task<CommandResult> Create(AzPostgresServerArcCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPostgresServerArcDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPostgresServerArcListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerArcListOptions(), token);
    }

    public async Task<CommandResult> Restore(AzPostgresServerArcRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPostgresServerArcShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPostgresServerArcUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}