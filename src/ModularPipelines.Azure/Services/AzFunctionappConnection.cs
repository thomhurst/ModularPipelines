using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzFunctionappConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListConfiguration(AzFunctionappConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListConfigurationOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListSupportTypes(AzFunctionappConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionListSupportTypesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzFunctionappConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Validate(AzFunctionappConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionValidateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzFunctionappConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionWaitOptions(), cancellationToken: token);
    }
}