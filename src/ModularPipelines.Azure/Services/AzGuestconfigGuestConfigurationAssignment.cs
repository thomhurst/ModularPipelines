using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig")]
public class AzGuestconfigGuestConfigurationAssignment
{
    public AzGuestconfigGuestConfigurationAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzGuestconfigGuestConfigurationAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzGuestconfigGuestConfigurationAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzGuestconfigGuestConfigurationAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGuestconfigGuestConfigurationAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationAssignmentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzGuestconfigGuestConfigurationAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationAssignmentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzGuestconfigGuestConfigurationAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationAssignmentWaitOptions(), token);
    }
}