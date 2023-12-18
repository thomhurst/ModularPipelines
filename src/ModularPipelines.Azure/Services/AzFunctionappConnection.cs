using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp")]
public class AzFunctionappConnection
{
    public AzFunctionappConnection(
        AzFunctionappConnectionCreate create,
        AzFunctionappConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappConnectionCreate Create { get; }

    public AzFunctionappConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzFunctionappConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzFunctionappConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListOptions(), token);
    }

    public async Task<CommandResult> ListConfiguration(AzFunctionappConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListConfigurationOptions(), token);
    }

    public async Task<CommandResult> ListSupportTypes(AzFunctionappConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzFunctionappConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzFunctionappConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionWaitOptions(), token);
    }
}