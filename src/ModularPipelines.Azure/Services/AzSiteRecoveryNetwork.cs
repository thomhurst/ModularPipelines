using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery")]
public class AzSiteRecoveryNetwork
{
    public AzSiteRecoveryNetwork(
        AzSiteRecoveryNetworkMapping mapping,
        ICommand internalCommand
    )
    {
        Mapping = mapping;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSiteRecoveryNetworkMapping Mapping { get; }

    public async Task<CommandResult> List(AzSiteRecoveryNetworkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSiteRecoveryNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryNetworkShowOptions(), token);
    }
}