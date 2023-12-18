using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConnection
{
    public AzConnection(
        AzConnectionCreate create,
        AzConnectionPreviewConfiguration previewConfiguration,
        AzConnectionUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        PreviewConfiguration = previewConfiguration;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConnectionCreate Create { get; }

    public AzConnectionPreviewConfiguration PreviewConfiguration { get; }

    public AzConnectionUpdate Update { get; }

    public async Task<CommandResult> Delete(AzConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateConfiguration(AzConnectionGenerateConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSupportTypes(AzConnectionListSupportTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionListSupportTypesOptions(), token);
    }

    public async Task<CommandResult> Show(AzConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Validate(AzConnectionValidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionValidateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionWaitOptions(), token);
    }
}