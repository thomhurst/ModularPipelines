using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ams")]
public class AzAmsStreamingLocator
{
    public AzAmsStreamingLocator(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAmsStreamingLocatorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsStreamingLocatorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingLocatorDeleteOptions(), token);
    }

    public async Task<CommandResult> GetPaths(AzAmsStreamingLocatorGetPathsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingLocatorGetPathsOptions(), token);
    }

    public async Task<CommandResult> List(AzAmsStreamingLocatorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListContentKeys(AzAmsStreamingLocatorListContentKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingLocatorListContentKeysOptions(), token);
    }

    public async Task<CommandResult> Show(AzAmsStreamingLocatorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingLocatorShowOptions(), token);
    }
}