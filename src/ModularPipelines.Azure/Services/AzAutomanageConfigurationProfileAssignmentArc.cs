using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment")]
public class AzAutomanageConfigurationProfileAssignmentArc
{
    public AzAutomanageConfigurationProfileAssignmentArc(
        AzAutomanageConfigurationProfileAssignmentArcReport report,
        ICommand internalCommand
    )
    {
        Report = report;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageConfigurationProfileAssignmentArcReport Report { get; }

    public async Task<CommandResult> Create(AzAutomanageConfigurationProfileAssignmentArcCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomanageConfigurationProfileAssignmentArcDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentArcDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzAutomanageConfigurationProfileAssignmentArcShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentArcShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAutomanageConfigurationProfileAssignmentArcUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentArcUpdateOptions(), token);
    }
}