using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzQuantum
{
    public AzQuantum(
        AzQuantumJob job,
        AzQuantumOfferings offerings,
        AzQuantumTarget target,
        AzQuantumWorkspace workspace,
        ICommand internalCommand
    )
    {
        Job = job;
        Offerings = offerings;
        Target = target;
        Workspace = workspace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzQuantumJob Job { get; }

    public AzQuantumOfferings Offerings { get; }

    public AzQuantumTarget Target { get; }

    public AzQuantumWorkspace Workspace { get; }

    public async Task<CommandResult> Execute(AzQuantumExecuteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(AzQuantumRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}