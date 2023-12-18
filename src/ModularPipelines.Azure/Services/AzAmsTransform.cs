using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams")]
public class AzAmsTransform
{
    public AzAmsTransform(
        AzAmsTransformOutput output,
        ICommand internalCommand
    )
    {
        Output = output;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAmsTransformOutput Output { get; }

    public async Task<CommandResult> Create(AzAmsTransformCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsTransformDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAmsTransformListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAmsTransformShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsTransformShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmsTransformUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsTransformUpdateOptions(), token);
    }
}