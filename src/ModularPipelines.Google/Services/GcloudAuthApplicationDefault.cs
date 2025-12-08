using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("auth")]
public class GcloudAuthApplicationDefault
{
    public GcloudAuthApplicationDefault(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Login(GcloudAuthApplicationDefaultLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAuthApplicationDefaultLoginOptions(), token);
    }

    public async Task<CommandResult> PrintAccessToken(GcloudAuthApplicationDefaultPrintAccessTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAuthApplicationDefaultPrintAccessTokenOptions(), token);
    }

    public async Task<CommandResult> Revoke(GcloudAuthApplicationDefaultRevokeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAuthApplicationDefaultRevokeOptions(), token);
    }

    public async Task<CommandResult> SetQuotaProject(GcloudAuthApplicationDefaultSetQuotaProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}