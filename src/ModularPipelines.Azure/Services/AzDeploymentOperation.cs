using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment")]
public class AzDeploymentOperation
{
    public AzDeploymentOperation(
        AzDeploymentOperationGroup group,
        AzDeploymentOperationMg mg,
        AzDeploymentOperationSub sub,
        AzDeploymentOperationTenant tenant,
        ICommand internalCommand
    )
    {
        Group = group;
        Mg = mg;
        Sub = sub;
        Tenant = tenant;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDeploymentOperationGroup Group { get; }

    public AzDeploymentOperationMg Mg { get; }

    public AzDeploymentOperationSub Sub { get; }

    public AzDeploymentOperationTenant Tenant { get; }

    public async Task<CommandResult> List(AzDeploymentOperationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDeploymentOperationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}