using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSpringConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListConfiguration(AzSpringConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListConfigurationOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListSupportTypes(AzSpringConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionListSupportTypesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSpringConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Validate(AzSpringConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionValidateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSpringConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionWaitOptions(), cancellationToken: token);
    }
}