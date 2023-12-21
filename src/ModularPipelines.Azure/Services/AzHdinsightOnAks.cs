using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzHdinsightOnAks
{
    public AzHdinsightOnAks(
        AzHdinsightOnAksCluster cluster,
        AzHdinsightOnAksClusterpool clusterpool,
        ICommand internalCommand
    )
    {
        Cluster = cluster;
        Clusterpool = clusterpool;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHdinsightOnAksCluster Cluster { get; }

    public AzHdinsightOnAksClusterpool Clusterpool { get; }

    public async Task<CommandResult> CheckNameAvailability(AzHdinsightOnAksCheckNameAvailabilityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksCheckNameAvailabilityOptions(), token);
    }

    public async Task<CommandResult> ListAvailableClusterPoolVersion(AzHdinsightOnAksListAvailableClusterPoolVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableClusterVersion(AzHdinsightOnAksListAvailableClusterVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}