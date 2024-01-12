using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudCloudShell
{
    public GcloudCloudShell(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetMountCommand(GcloudCloudShellGetMountCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scp(GcloudCloudShellScpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ssh(GcloudCloudShellSshOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudCloudShellSshOptions(), token);
    }
}