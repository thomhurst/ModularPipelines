using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAnthos
{
    public GcloudAnthos(
        GcloudAnthosAuth auth,
        GcloudAnthosConfig config,
        ICommand internalCommand
    )
    {
        Auth = auth;
        Config = config;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAnthosAuth Auth { get; }

    public GcloudAnthosConfig Config { get; }

    public async Task<CommandResult> CreateLoginConfig(GcloudAnthosCreateLoginConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}