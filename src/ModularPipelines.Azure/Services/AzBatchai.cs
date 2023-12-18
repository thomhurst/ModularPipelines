using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzBatchai
{
    public AzBatchai(
        AzBatchaiCluster cluster,
        AzBatchaiExperiment experiment,
        AzBatchaiFileServer fileServer,
        AzBatchaiJob job,
        AzBatchaiWorkspace workspace,
        ICommand internalCommand
    )
    {
        Cluster = cluster;
        Experiment = experiment;
        FileServer = fileServer;
        Job = job;
        Workspace = workspace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchaiCluster Cluster { get; }

    public AzBatchaiExperiment Experiment { get; }

    public AzBatchaiFileServer FileServer { get; }

    public AzBatchaiJob Job { get; }

    public AzBatchaiWorkspace Workspace { get; }

    public async Task<CommandResult> ListUsages(AzBatchaiListUsagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}