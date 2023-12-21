using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp")]
public class AzFunctionappLog
{
    public AzFunctionappLog(
        AzFunctionappLogDeployment deployment
    )
    {
        Deployment = deployment;
    }

    public AzFunctionappLogDeployment Deployment { get; }
}