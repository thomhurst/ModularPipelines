using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emulators")]
public class GcloudEmulatorsSpanner
{
    public GcloudEmulatorsSpanner(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EnvInit(GcloudEmulatorsSpannerEnvInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEmulatorsSpannerEnvInitOptions(), token);
    }

    public async Task<CommandResult> Start(GcloudEmulatorsSpannerStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEmulatorsSpannerStartOptions(), token);
    }
}