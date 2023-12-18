using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confluent")]
public class AzConfluentOrganization
{
    public AzConfluentOrganization(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConfluentOrganizationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConfluentOrganizationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfluentOrganizationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConfluentOrganizationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfluentOrganizationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConfluentOrganizationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfluentOrganizationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConfluentOrganizationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfluentOrganizationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConfluentOrganizationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfluentOrganizationWaitOptions(), token);
    }
}

