using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd")]
public class AzAfdCustomDomain
{
    public AzAfdCustomDomain(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAfdCustomDomainCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAfdCustomDomainDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdCustomDomainDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAfdCustomDomainListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RegenerateValidationToken(AzAfdCustomDomainRegenerateValidationTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdCustomDomainRegenerateValidationTokenOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAfdCustomDomainShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdCustomDomainShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAfdCustomDomainUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdCustomDomainUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzAfdCustomDomainWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAfdCustomDomainWaitOptions(), cancellationToken: token);
    }
}