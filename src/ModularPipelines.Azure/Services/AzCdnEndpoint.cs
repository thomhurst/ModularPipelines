using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn")]
public class AzCdnEndpoint
{
    public AzCdnEndpoint(
        AzCdnEndpointRule rule,
        AzCdnEndpointWaf waf,
        ICommand internalCommand
    )
    {
        Rule = rule;
        Waf = waf;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCdnEndpointRule Rule { get; }

    public AzCdnEndpointWaf Waf { get; }

    public async Task<CommandResult> Create(AzCdnEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCdnEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzCdnEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Load(AzCdnEndpointLoadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Purge(AzCdnEndpointPurgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCdnEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzCdnEndpointStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzCdnEndpointStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzCdnEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointUpdateOptions(), token);
    }

    public async Task<CommandResult> ValidateCustomDomain(AzCdnEndpointValidateCustomDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}