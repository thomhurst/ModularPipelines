using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment")]
public class AzAutomanageConfigurationProfileAssignmentCluster
{
    public AzAutomanageConfigurationProfileAssignmentCluster(
        AzAutomanageConfigurationProfileAssignmentClusterReport report,
        ICommand internalCommand
    )
    {
        Report = report;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageConfigurationProfileAssignmentClusterReport Report { get; }

    public async Task<CommandResult> Create(AzAutomanageConfigurationProfileAssignmentClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomanageConfigurationProfileAssignmentClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentClusterDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzAutomanageConfigurationProfileAssignmentClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentClusterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAutomanageConfigurationProfileAssignmentClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentClusterUpdateOptions(), token);
    }
}