using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "container")]
public class AzFunctionappConfigContainerSet
{
    public AzFunctionappConfigContainerSet(
        AzFunctionappConfigContainerSetAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzFunctionappConfigContainerSetAppserviceKube AppserviceKube { get; }
}