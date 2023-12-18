using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "plan")]
public class AzAppservicePlanShow
{
    public AzAppservicePlanShow(
        AzAppservicePlanShowAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzAppservicePlanShowAppserviceKube AppserviceKube { get; }
}