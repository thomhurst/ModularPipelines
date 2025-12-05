using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "service-properties", "update")]
public class AzStorageBlobServicePropertiesUpdateStorageBlobPreview
{
    public AzStorageBlobServicePropertiesUpdateStorageBlobPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzStorageBlobServicePropertiesUpdateStorageBlobPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobServicePropertiesUpdateStorageBlobPreviewExtensionOptions(), token);
    }
}