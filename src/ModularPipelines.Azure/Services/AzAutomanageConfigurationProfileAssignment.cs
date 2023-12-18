using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage")]
public class AzAutomanageConfigurationProfileAssignment
{
    public AzAutomanageConfigurationProfileAssignment(
        AzAutomanageConfigurationProfileAssignmentArc arc,
        AzAutomanageConfigurationProfileAssignmentCluster cluster,
        AzAutomanageConfigurationProfileAssignmentVm vm,
        ICommand internalCommand
    )
    {
        Arc = arc;
        Cluster = cluster;
        Vm = vm;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageConfigurationProfileAssignmentArc Arc { get; }

    public AzAutomanageConfigurationProfileAssignmentCluster Cluster { get; }

    public AzAutomanageConfigurationProfileAssignmentVm Vm { get; }

    public async Task<CommandResult> List(AzAutomanageConfigurationProfileAssignmentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentListOptions(), token);
    }
}