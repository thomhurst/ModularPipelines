using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp")]
public class AzLogicappDeployment
{
    public AzLogicappDeployment(
        AzLogicappDeploymentSource source
    )
    {
        Source = source;
    }

    public AzLogicappDeploymentSource Source { get; }
}