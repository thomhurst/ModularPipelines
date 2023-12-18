using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzDeployment
{
    public AzDeployment(
        AzDeploymentGroup group,
        AzDeploymentMg mg,
        AzDeploymentOperation operation,
        AzDeploymentSub sub,
        AzDeploymentTenant tenant,
        ICommand internalCommand
    )
    {
        Group = group;
        Mg = mg;
        Operation = operation;
        Sub = sub;
        Tenant = tenant;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDeploymentGroup Group { get; }

    public AzDeploymentMg Mg { get; }

    public AzDeploymentOperation Operation { get; }

    public AzDeploymentSub Sub { get; }

    public AzDeploymentTenant Tenant { get; }

    public async Task<CommandResult> Cancel(AzDeploymentCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDeploymentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDeploymentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(AzDeploymentExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDeploymentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDeploymentShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Validate(AzDeploymentValidateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDeploymentWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

