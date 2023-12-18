using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNginxDeploymentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNginxDeploymentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNginxDeploymentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNginxDeploymentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNginxDeploymentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentWaitOptions(), token);
    }
}