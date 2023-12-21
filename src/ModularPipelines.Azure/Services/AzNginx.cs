using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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