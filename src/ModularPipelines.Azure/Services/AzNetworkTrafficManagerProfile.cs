using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager")]
public class AzNetworkTrafficManagerProfile
{
    public AzNetworkTrafficManagerProfile(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CheckDns(AzNetworkTrafficManagerProfileCheckDnsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerProfileCheckDnsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzNetworkTrafficManagerProfileCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkTrafficManagerProfileDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerProfileDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkTrafficManagerProfileListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerProfileListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkTrafficManagerProfileShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerProfileShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkTrafficManagerProfileUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerProfileUpdateOptions(), cancellationToken: token);
    }
}