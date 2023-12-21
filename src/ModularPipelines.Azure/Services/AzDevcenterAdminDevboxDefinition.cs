using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin")]
public class AzDevcenterAdminDevboxDefinition
{
    public AzDevcenterAdminDevboxDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminDevboxDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminDevboxDefinitionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminDevboxDefinitionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminDevboxDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminDevboxDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminDevboxDefinitionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminDevboxDefinitionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminDevboxDefinitionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminDevboxDefinitionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminDevboxDefinitionWaitOptions(), token);
    }
}