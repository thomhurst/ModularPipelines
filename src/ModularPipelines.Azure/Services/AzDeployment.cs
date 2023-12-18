using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDeployment
{
    public AzDeployment(
        AzDeploymentGroup group,
        AzDeploymentMg mg,
        AzDeploymentOperation operation,
        AzDeploymentSub sub,
        AzDeploymentTenant tenant
    )
    {
        Group = group;
        Mg = mg;
        Operation = operation;
        Sub = sub;
        Tenant = tenant;
    }

    public AzDeploymentGroup Group { get; }

    public AzDeploymentMg Mg { get; }

    public AzDeploymentOperation Operation { get; }

    public AzDeploymentSub Sub { get; }

    public AzDeploymentTenant Tenant { get; }
}