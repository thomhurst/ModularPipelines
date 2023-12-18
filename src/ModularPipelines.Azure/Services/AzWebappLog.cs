using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp")]
public class AzWebappLog
{
    public AzWebappLog(
        AzWebappLogDeployment deployment,
        ICommand internalCommand
    )
    {
        Deployment = deployment;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappLogDeployment Deployment { get; }

    public async Task<CommandResult> Config(AzWebappLogConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappLogConfigOptions(), token);
    }

    public async Task<CommandResult> Download(AzWebappLogDownloadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappLogDownloadOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappLogShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappLogShowOptions(), token);
    }

    public async Task<CommandResult> Tail(AzWebappLogTailOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappLogTailOptions(), token);
    }
}