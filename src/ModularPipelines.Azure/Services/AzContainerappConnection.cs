using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappConnection
{
    public AzContainerappConnection(
        AzContainerappConnectionCreate create,
        AzContainerappConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappConnectionCreate Create { get; }

    public AzContainerappConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzContainerappConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzContainerappConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionListOptions(), token);
    }

    public async Task<CommandResult> ListConfiguration(AzContainerappConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionListConfigurationOptions(), token);
    }

    public async Task<CommandResult> ListSupportTypes(AzContainerappConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzContainerappConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzContainerappConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzContainerappConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionWaitOptions(), token);
    }
}