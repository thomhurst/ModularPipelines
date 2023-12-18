using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment")]
public class AzNginxDeploymentCertificate
{
    public AzNginxDeploymentCertificate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNginxDeploymentCertificateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNginxDeploymentCertificateDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentCertificateDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNginxDeploymentCertificateListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNginxDeploymentCertificateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentCertificateShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNginxDeploymentCertificateUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentCertificateUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNginxDeploymentCertificateWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentCertificateWaitOptions(), token);
    }
}