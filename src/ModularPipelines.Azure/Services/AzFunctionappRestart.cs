using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp")]
public class AzFunctionappRestart
{
    public AzFunctionappRestart(
        AzFunctionappRestartAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzFunctionappRestartAppserviceKube AppserviceKube { get; }
}