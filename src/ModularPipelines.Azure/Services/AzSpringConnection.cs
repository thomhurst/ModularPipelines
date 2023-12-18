using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring")]
public class AzSpringConnection
{
    public AzSpringConnection(
        AzSpringConnectionCreate create,
        AzSpringConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringConnectionCreate Create { get; }

    public AzSpringConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzSpringConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSpringConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListOptions(), token);
    }

    public async Task<CommandResult> ListConfiguration(AzSpringConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListConfigurationOptions(), token);
    }

    public async Task<CommandResult> ListSupportTypes(AzSpringConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzSpringConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzSpringConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSpringConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionWaitOptions(), token);
    }
}