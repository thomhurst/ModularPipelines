using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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