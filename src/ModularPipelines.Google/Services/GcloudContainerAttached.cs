using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container")]
public class GcloudContainerAttached
{
    public GcloudContainerAttached(
        GcloudContainerAttachedClusters clusters,
        GcloudContainerAttachedOperations operations,
        ICommand internalCommand
    )
    {
        Clusters = clusters;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerAttachedClusters Clusters { get; }

    public GcloudContainerAttachedOperations Operations { get; }

    public async Task<CommandResult> GetServerConfig(GcloudContainerAttachedGetServerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerAttachedGetServerConfigOptions(), token);
    }
}