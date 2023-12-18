using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

