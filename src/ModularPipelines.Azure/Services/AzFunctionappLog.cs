using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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