using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks")]
public class AzHdinsightOnAksCluster
{
    public AzHdinsightOnAksCluster(
        AzHdinsightOnAksClusterJob job,
        AzHdinsightOnAksClusterNodeProfile nodeProfile,
        AzHdinsightOnAksClusterSecret secret,
        AzHdinsightOnAksClusterTrinoHiveCatalog trinoHiveCatalog,
        ICommand internalCommand
    )
    {
        Job = job;
        NodeProfile = nodeProfile;
        Secret = secret;
        TrinoHiveCatalog = trinoHiveCatalog;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHdinsightOnAksClusterJob Job { get; }

    public AzHdinsightOnAksClusterNodeProfile NodeProfile { get; }

    public AzHdinsightOnAksClusterSecret Secret { get; }

    public AzHdinsightOnAksClusterTrinoHiveCatalog TrinoHiveCatalog { get; }

    public async Task<CommandResult> Create(AzHdinsightOnAksClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHdinsightOnAksClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzHdinsightOnAksClusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServiceConfig(AzHdinsightOnAksClusterListServiceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(AzHdinsightOnAksClusterResizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterResizeOptions(), token);
    }

    public async Task<CommandResult> Show(AzHdinsightOnAksClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterShowOptions(), token);
    }

    public async Task<CommandResult> ShowInstanceView(AzHdinsightOnAksClusterShowInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterShowInstanceViewOptions(), token);
    }

    public async Task<CommandResult> Update(AzHdinsightOnAksClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzHdinsightOnAksClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightOnAksClusterWaitOptions(), token);
    }
}

