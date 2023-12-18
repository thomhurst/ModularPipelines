using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudConnection
{
    public AzSpringCloudConnection(
        AzSpringCloudConnectionCreate create,
        AzSpringCloudConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudConnectionCreate Create { get; }

    public AzSpringCloudConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzSpringCloudConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSpringCloudConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionListOptions(), token);
    }

    public async Task<CommandResult> ListConfiguration(AzSpringCloudConnectionListConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionListConfigurationOptions(), token);
    }

    public async Task<CommandResult> ListSupportTypes(AzSpringCloudConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzSpringCloudConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzSpringCloudConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSpringCloudConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionWaitOptions(), token);
    }
}

