using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin")]
public class AzDevcenterAdminProjectEnvironmentType
{
    public AzDevcenterAdminProjectEnvironmentType(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminProjectEnvironmentTypeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminProjectEnvironmentTypeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminProjectEnvironmentTypeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminProjectEnvironmentTypeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminProjectEnvironmentTypeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminProjectEnvironmentTypeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminProjectEnvironmentTypeUpdateOptions(), token);
    }
}