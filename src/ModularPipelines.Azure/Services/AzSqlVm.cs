using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlVm
{
    public AzSqlVm(
        AzSqlVmGroup group,
        ICommand internalCommand
    )
    {
        Group = group;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlVmGroup Group { get; }

    public async Task<CommandResult> AddToGroup(AzSqlVmAddToGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSqlVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmDeleteOptions(), token);
    }

    public async Task<CommandResult> EnableAzureAdAuth(AzSqlVmEnableAzureAdAuthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmEnableAzureAdAuthOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmListOptions(), token);
    }

    public async Task<CommandResult> RemoveFromGroup(AzSqlVmRemoveFromGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmRemoveFromGroupOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmShowOptions(), token);
    }

    public async Task<CommandResult> StartAssessment(AzSqlVmStartAssessmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmStartAssessmentOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmUpdateOptions(), token);
    }

    public async Task<CommandResult> ValidateAzureAdAuth(AzSqlVmValidateAzureAdAuthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmValidateAzureAdAuthOptions(), token);
    }
}