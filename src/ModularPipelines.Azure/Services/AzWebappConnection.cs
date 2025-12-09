using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp")]
public class AzWebappConnection
{
    public AzWebappConnection(
        AzWebappConnectionCreate create,
        AzWebappConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappConnectionCreate Create { get; }

    public AzWebappConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzWebappConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzWebappConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionListOptions(), token);
    }

    public async Task<CommandResult> ListConfiguration(AzWebappConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionListConfigurationOptions(), token);
    }

    public async Task<CommandResult> ListSupportTypes(AzWebappConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzWebappConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWebappConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionWaitOptions(), token);
    }
}