using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "plan")]
public class AzAppservicePlanUpdate
{
    public AzAppservicePlanUpdate(
        AzAppservicePlanUpdateAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzAppservicePlanUpdateAppserviceKube AppserviceKube { get; }
}