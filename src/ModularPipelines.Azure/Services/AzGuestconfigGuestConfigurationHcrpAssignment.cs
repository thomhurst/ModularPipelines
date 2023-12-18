using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig")]
public class AzGuestconfigGuestConfigurationHcrpAssignment
{
    public AzGuestconfigGuestConfigurationHcrpAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzGuestconfigGuestConfigurationHcrpAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzGuestconfigGuestConfigurationHcrpAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationHcrpAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzGuestconfigGuestConfigurationHcrpAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGuestconfigGuestConfigurationHcrpAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationHcrpAssignmentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzGuestconfigGuestConfigurationHcrpAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationHcrpAssignmentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzGuestconfigGuestConfigurationHcrpAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationHcrpAssignmentWaitOptions(), token);
    }
}