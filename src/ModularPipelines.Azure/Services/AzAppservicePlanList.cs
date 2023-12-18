using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "plan")]
public class AzAppservicePlanList
{
    public AzAppservicePlanList(
        AzAppservicePlanListAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzAppservicePlanListAppserviceKube AppserviceKube { get; }
}