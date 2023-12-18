using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana")]
public class AzGrafanaServiceAccount
{
    public AzGrafanaServiceAccount(
        AzGrafanaServiceAccountToken token,
        ICommand internalCommand
    )
    {
        Token = token;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzGrafanaServiceAccountToken Token { get; }

    public async Task<CommandResult> Create(AzGrafanaServiceAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzGrafanaServiceAccountDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzGrafanaServiceAccountListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGrafanaServiceAccountShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzGrafanaServiceAccountUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}