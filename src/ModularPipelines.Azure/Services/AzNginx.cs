using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNginx
{
    public AzNginx(
        AzNginxDeployment deployment
    )
    {
        Deployment = deployment;
    }

    public AzNginxDeployment Deployment { get; }
}