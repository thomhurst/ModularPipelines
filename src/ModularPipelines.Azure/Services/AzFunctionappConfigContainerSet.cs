using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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