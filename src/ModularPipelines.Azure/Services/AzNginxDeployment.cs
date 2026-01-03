using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("nginx")]
public class AzNginxDeployment
{
    public AzNginxDeployment(
        AzNginxDeploymentCertificate certificate,
        AzNginxDeploymentConfiguration configuration,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        Configuration = configuration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNginxDeploymentCertificate Certificate { get; }

    public AzNginxDeploymentConfiguration Configuration { get; }

    public async Task<CommandResult> Create(AzNginxDeploymentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNginxDeploymentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNginxDeploymentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNginxDeploymentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNginxDeploymentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNginxDeploymentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentWaitOptions(), cancellationToken: token);
    }
}