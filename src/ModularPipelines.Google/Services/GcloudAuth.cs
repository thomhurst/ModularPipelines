using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAuth
{
    public GcloudAuth(
        GcloudAuthApplicationDefault applicationDefault,
        GcloudAuthEnterpriseCertificateConfig enterpriseCertificateConfig,
        ICommand internalCommand
    )
    {
        ApplicationDefault = applicationDefault;
        EnterpriseCertificateConfig = enterpriseCertificateConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAuthApplicationDefault ApplicationDefault { get; }

    public GcloudAuthEnterpriseCertificateConfig EnterpriseCertificateConfig { get; }

    public async Task<CommandResult> ActivateServiceAccount(GcloudAuthActivateServiceAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigureDocker(GcloudAuthConfigureDockerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAuthListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAuthListOptions(), token);
    }

    public async Task<CommandResult> Login(GcloudAuthLoginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PrintAccessToken(GcloudAuthPrintAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PrintIdentityToken(GcloudAuthPrintIdentityTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Revoke(GcloudAuthRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}