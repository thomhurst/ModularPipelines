using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment")]
public class AzAutomanageConfigurationProfileAssignmentVm
{
    public AzAutomanageConfigurationProfileAssignmentVm(
        AzAutomanageConfigurationProfileAssignmentVmReport report,
        ICommand internalCommand
    )
    {
        Report = report;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageConfigurationProfileAssignmentVmReport Report { get; }

    public async Task<CommandResult> Create(AzAutomanageConfigurationProfileAssignmentVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomanageConfigurationProfileAssignmentVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentVmDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzAutomanageConfigurationProfileAssignmentVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentVmShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAutomanageConfigurationProfileAssignmentVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileAssignmentVmUpdateOptions(), token);
    }
}