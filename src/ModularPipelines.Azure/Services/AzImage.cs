using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzImage
{
    public AzImage(
        AzImageBuilder builder,
        ICommand internalCommand
    )
    {
        Builder = builder;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzImageBuilder Builder { get; }

    public async Task<CommandResult> Copy(AzImageCopyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzImageCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzImageDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzImageListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageListOptions(), token);
    }

    public async Task<CommandResult> Show(AzImageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzImageUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzImageWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageWaitOptions(), token);
    }
}