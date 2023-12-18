using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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