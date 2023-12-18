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
        AzDeploymentOperationTenant tenant
    )
    {
        Group = group;
        Mg = mg;
        Sub = sub;
        Tenant = tenant;
    }

    public AzDeploymentOperationGroup Group { get; }

    public AzDeploymentOperationMg Mg { get; }

    public AzDeploymentOperationSub Sub { get; }

    public AzDeploymentOperationTenant Tenant { get; }
}