using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver")]
public class AzDnsResolverVnetLink
{
    public AzDnsResolverVnetLink(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDnsResolverVnetLinkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDnsResolverVnetLinkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverVnetLinkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDnsResolverVnetLinkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDnsResolverVnetLinkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverVnetLinkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDnsResolverVnetLinkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverVnetLinkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDnsResolverVnetLinkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverVnetLinkWaitOptions(), token);
    }
}